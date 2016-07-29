using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    class Inventory
    {
        private Dictionary<Product, int> _productCounts = new Dictionary<Product, int>();

        public Inventory(ICheckoutEventCreator checkoutCreator)
        {
            InitializeProducts();

            checkoutCreator.OnPreCheckoutEvent += OnPreCheckout;
            checkoutCreator.OnCheckoutEvent += OnCheckout;
            // No need to register for OnPostCheckoutEvent
        }

        private void InitializeProducts()
        {
            Product[] products = new Product[]
            {
                new Product("Self-sealing stem bolt (144 pack)", "Unknown", 35.99),
                new Product("Reverse-ratcheting routing planer", "Unknown", 12.97),
                new Product("Yamok sauce (10 wrappages)", "Food", 6.99),
                new Product("Gagh", "Food", 0.03),
                new Product("Klingon bloodwine", "Food", 10.00),
                new Product("Romulan ale", "Food", 3599.00),
                new Product("Alphanumeric sequencer", "Engineering Tools", 531.75),
                new Product("Quantum flux regulator", "Engineering Tools", 997.97),
                new Product("Thermal regulator", "Engineering Tools", 222.22)
            };
            int[] productCounts = new int[]
            {
                12000,
                3000,
                1000,
                1000,
                300,
                0,
                74,
                27,
                42
            };
            for (int i = 0; i < products.Length; i++)
            {
                Product product = products[i];
                int count = productCounts[i];

                AddProduct(product, count);
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
            bool result;
            int available = GetProductCount(product);
            errorMsg = null;

            if (available == 0)
            {
                errorMsg = String.Format("'{0}' is out of stock.", product.Name);
                result = false;
            }
            else if (available < count)
            {
                errorMsg = String.Format("'{0}' has fewer than {1} units in stock", product.Name, count);
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
            int oldCount = GetProductCount(product);
            _productCounts[product] = oldCount + count;
        }

        private int GetProductCount(Product product)
        {
            return DictionaryHelper.GetWithDefault(_productCounts, product);
        }

        private void OnPreCheckout(User.ShoppingCart cart, AddOnlyCollection<String> errors)
        {
            // Ensure that every item in the cart has the necessary amout in stock.
            foreach (Product product in cart.GetAllProducts())
            {
                int needed = cart.GetProductCount(product);
                if (!CheckAvailability(product, needed))
                {
                    String name = product.Name;
                    if (GetProductCount(product) == 0)
                    {
                        errors.Add(String.Format("'{0}' is out of stock.", name));
                    }
                    else
                    {
                        errors.Add(String.Format("'{0}' has fewer than {1} units in stock", name, needed));
                    }
                }
            }
        }

        private void OnCheckout(User.ShoppingCart cart)
        {
            // Remove the appropriate number of units of each item in the cart from inventory.
            foreach (Product product in cart.GetAllProducts())
            {
                _productCounts[product] -= cart.GetProductCount(product);
            }
        }
    }
}
