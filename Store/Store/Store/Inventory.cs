using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StoreProgram.Store
{
    class Inventory
    {
        private Dictionary<Product, int> _productCounts = new Dictionary<Product, int>();
        private String _filepath;

        // Key = transaction ID
        // Value = Dictionary
        //         {
        //              Key = product
        //              Value = number of product to reserve
        //          }
        private Dictionary<int, Dictionary<Product, int>> _reservations = new Dictionary<int, Dictionary<Product, int>>();

        public Inventory(String filepath, ICheckoutEventCreator checkoutCreator)
        {
            _filepath = filepath;
            LoadProducts();

            checkoutCreator.OnPreReserveEvent += OnPreReserve;
            checkoutCreator.OnReserveEvent += OnReserve;
            checkoutCreator.OnPreCheckoutEvent += OnPreCheckout;
            checkoutCreator.OnCheckoutEvent += OnCheckout;
            checkoutCreator.OnTransactionEndedEvent += OnTransactionEndedAsync;
        }

        private void LoadProducts()
        {
            using (var fileStream = File.Open(_filepath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    while (!(reader.EndOfStream))
                    {
                        String line = reader.ReadLine();
                        Queue<String> tokens = new Queue<string>(line.Split('\0'));
                        Product product = new Product(tokens);

                        // Throws exception if there aren't enough tokens or
                        // token does not represent an integer.
                        int count = int.Parse(tokens.Dequeue());

                        AddProduct(product, count);
                    }
                }
            }


            //Product[] products = new Product[]
            //{
            //    new Product("Self-sealing stem bolt (144 pack)", "Unknown", 35.99),
            //    new Product("Reverse-ratcheting routing planer", "Unknown", 12.97),
            //    new Product("Yamok sauce (10 wrappages)", "Food", 6.99),
            //    new Product("Gagh", "Food", 0.03),
            //    new Product("Klingon bloodwine", "Food", 10.00),
            //    new Product("Romulan ale", "Food", 3599.00),
            //    new Product("Alphanumeric sequencer", "Engineering Tools", 531.75),
            //    new Product("Quantum flux regulator", "Engineering Tools", 997.97),
            //    new Product("Thermal regulator", "Engineering Tools", 222.22)
            //};
            //int[] productCounts = new int[]
            //{
            //    12000,
            //    3000,
            //    1000,
            //    1000,
            //    300,
            //    0,
            //    74,
            //    27,
            //    42
            //};
            //for (int i = 0; i < products.Length; i++)
            //{
            //    Product product = products[i];
            //    int count = productCounts[i];

            //    AddProduct(product, count);
            //}
        }

        private async Task SaveProductsAsync()
        {
            using (var fileStream = File.Open(_filepath, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    foreach (KeyValuePair<Product, int> pair in _productCounts)
                    {
                        Product product = pair.Key;
                        int count = pair.Value;
                        await writer.WriteLineAsync(String.Format("{0}\0{1}", product.ToStorageString(), count));
                    }
                }
            }
            
        }

        public bool CheckAvailability(uint productId, int count = 1)
        {
            String errorMsg;
            return CheckAvailability(productId, count, out errorMsg);
        }

        public bool CheckAvailability(uint productId, int count, out String errorMsg)
        {
            return CheckAvailability(GetProduct(productId), count, out errorMsg);
        }

        public bool CheckAvailability(Product product, int count = 1)
        {
            String errorMsg;
            return CheckAvailability(product, count, out errorMsg);
        }

        public bool CheckAvailability(Product product, int count, out String errorMsg)
        {
            return CheckAvailability(product, count, false, out errorMsg);
        }

        private bool CheckAvailability(Product product, int count, bool ignoreReservations, out String errorMsg)
        {
            bool result;
            int available = ignoreReservations ? GetFullProductCount(product) : GetUnreservedProductCount(product);
            errorMsg = null;

            if (available == 0)
            {
                errorMsg = String.Format("'{0}' is out of stock.", product.Name);
                result = false;
            }
            else if (available < count)
            {
                errorMsg = String.Format("'{0}' has fewer than {1}{2} units in stock", 
                    product.Name, count, ignoreReservations ? "" : " unreserved");
                result = false;
            }
            else
            {
                // We have enough units in stock
                result = true;
            }

            return result;
        }

        public List<Product> GetAllProducts()
        {
            return new List<Product>(_productCounts.Keys);
        }

        public Product GetProduct(uint productId)
        {
            foreach (Product product in _productCounts.Keys)
            {
                if (product.Id == productId)
                {
                    return product;
                }
            }
            return null;
        }


        private void AddProduct(Product product, int count = 1)
        {
            int oldCount = GetFullProductCount(product);
            _productCounts[product] = oldCount + count;
        }

        private int GetUnreservedProductCount(Product product)
        {
            return GetFullProductCount(product) - GetReservedCount(product);
        }

        private int GetFullProductCount(Product product)
        {
            return DictionaryHelper.GetWithDefault(_productCounts, product);
        }

        private int GetReservedCount(Product product)
        {
            return _reservations.Values.SelectMany(x => x).Where(x => x.Key == product).Sum(x => x.Value);
        }

        private Dictionary<Product, int> GetOrCreateReservation(int transactionId)
        {
            Dictionary<Product, int> reservation = DictionaryHelper.GetWithDefault(_reservations, transactionId);
            if (reservation == null)
            {
                reservation = new Dictionary<Product, int>();
                _reservations[transactionId] = reservation;
            }

            return reservation;
        }

        private void OnPreReserve(Product product, int count, int transactionId, AddOnlyCollection<String> errors)
        {
            // Ensure that every item in the cart has the necessary amout in stock
            // and not already reserved.
            String errorMsg;
            if (!CheckAvailability(product, count, false, out errorMsg))
            {
                errors.Add(errorMsg);
            }
        }

        private void OnReserve(Product product, int count, int transactionId)
        {
            Dictionary<Product, int> reservation = GetOrCreateReservation(transactionId);
            int oldCount = DictionaryHelper.GetWithDefault(reservation, product, 0);
            reservation[product] = oldCount + count;

        }

        private void OnPreRelease(Product product, int count, int transactionId, AddOnlyCollection<int> maxReleased)
        {
            // How many do we actually have reserved?
            int numReserved = 0;
            Dictionary<Product, int> reservation = DictionaryHelper.GetWithDefault(_reservations, transactionId);
            if (reservation != null)
            {
                numReserved = DictionaryHelper.GetWithDefault(reservation, product, 0);
            }

            // Are we trying to release more than are reserved?
            if (numReserved < count)
            {
                // Yes. Tell the event publisher how many we can actually release.
                maxReleased.Add(numReserved);
            }

        }

        private void OnRelease(Product product, int count, int transactionId)
        {
            if (count == 0) return;

            // These keys should exist because we checked during OnPreRelease.
            var reservation = _reservations[transactionId];
            reservation[product] -= count;
        }

        private void OnPreCheckout(User.ShoppingCart cart, int transactionId, AddOnlyCollection<String> errors)
        {
            // Ensure that every item in the cart has the necessary amout in stock,
            // regardless of reservations.
            foreach (Product product in cart.GetAllProducts())
            {
                int needed = cart.GetProductCount(product);
                String errorMsg;
                if (!CheckAvailability(product, needed, true, out errorMsg))
                {
                    errors.Add(errorMsg);
                }
            }
        }

        private void OnCheckout(User.ShoppingCart cart, int transactionId)
        {
            // Remove the appropriate number of units of each item in the cart from inventory.
            foreach (Product product in cart.GetAllProducts())
            {
                _productCounts[product] -= cart.GetProductCount(product);
            }
        }

        private async void OnTransactionEndedAsync(int transactionId)
        {
            // Remove the given reservation if it exists.
            if (_reservations.ContainsKey(transactionId))
            {
                _reservations.Remove(transactionId);
            }

            // There have likely been changes to the inventory. Save to file. 
            await SaveProductsAsync();
        }
    }
}
