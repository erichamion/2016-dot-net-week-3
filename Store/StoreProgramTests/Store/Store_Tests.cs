using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreProgram.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StoreProgram.Store.Tests
{
    [TestClass()]
    public class Store_Tests
    {
        private Product[] _products =
            {
                new Product("Product 1.1", "Category 1", 10.00),
                new Product("Product 1.2", "Category 1", 10.00),
                new Product("Product 1.3", "Category 1", 10.00),
                new Product("Product 2.1", "Category 2", 10.00),
                new Product("Product 2.2", "Category 2", 10.00),
                new Product("Product 2.3", "Category 2", 10.00),
                new Product("Product 3.1", "Category 3", 10.00),
                new Product("Product 3.2", "Category 3", 10.00),
                new Product("Product 3.3", "Category 3", 10.00)
            };
        private int[] _counts = { 8, 7, 6, 5, 4, 3, 2, 1, 0 };

        private const String INVENTORY_PATH = "store.inv";

        [TestInitialize()]
        public void Init()
        {
            CreateInventoryFile();
        }

        
        [TestCleanup()]
        public void TearDown()
        {
            File.Delete(INVENTORY_PATH);
        }

        [TestMethod()]
        public void ReserveProducts_ValidArguments_Succeeds()
        {
            // Arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product productToReserve1 = pair.Key;
            int reserveCount1 = pair.Value / 2;
            pair = GetValidReserveArguments(1);
            Product productToReserve2 = pair.Key;
            int reserveCount2 = pair.Value / 2;
            String msg;
            bool success1;
            bool success2;
            List<Product> cartProducts;


            // Act
            success1 = store.ReserveProducts(productToReserve1, reserveCount1, out msg);
            success2 = store.ReserveProducts(productToReserve2, reserveCount2, out msg);
            cartProducts = store.Customer.Cart.GetAllProducts();

            // Assert
            Assert.IsTrue(success1);
            Assert.IsTrue(success2);
        }

        [TestMethod()]
        public void ReserveProducts_ValidArguments_AddsToCart()
        {
            // Arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product productToReserve1 = pair.Key;
            int reserveCount1 = pair.Value / 2;
            pair = GetValidReserveArguments(1);
            Product productToReserve2 = pair.Key;
            int reserveCount2 = pair.Value / 2;
            String msg;
            bool success1;
            bool success2;
            List<Product> cartProducts;

            // Act
            success1 = store.ReserveProducts(productToReserve1, reserveCount1, out msg);
            success2 = store.ReserveProducts(productToReserve2, reserveCount2, out msg);
            cartProducts = store.Customer.Cart.GetAllProducts();

            // Assert
            Assert.AreEqual(2, cartProducts.Count);
            Assert.IsTrue(cartProducts.Contains(productToReserve1));
            Assert.IsTrue(cartProducts.Contains(productToReserve2));
            Assert.AreEqual(reserveCount1, store.Customer.Cart.GetProductCount(productToReserve1));
            Assert.AreEqual(reserveCount2, store.Customer.Cart.GetProductCount(productToReserve2));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ReserveProducts_NegativeCount_ThrowsArgumentException()
        {
            // Arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments();
            Product productToReserve = pair.Key;
            int reserveCount = -3;
            String msg;
            bool success;
            
            // Act
            success = store.ReserveProducts(productToReserve, reserveCount, out msg);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void ReserveProducts_NegativeCount_CartNotModified()
        {
            // Arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product validProductToReserve = pair.Key;
            int reserveCount = pair.Value / 2;
            pair = GetValidReserveArguments(1);
            Product secondProductToReserve = pair.Key;
            int badCount = -3;
            String msg;
            bool validSuccess;
            List<Product> productsInCart = null;

            // Act
            validSuccess = store.ReserveProducts(validProductToReserve, reserveCount, out msg);
            try
            {
                bool success = store.ReserveProducts(secondProductToReserve, badCount, out msg);
                Assert.Fail("No exception thrown");
            }
            catch (ArgumentException)
            {
                productsInCart = store.Customer.Cart.GetAllProducts();
            }

            // Assert
            Assert.IsTrue(validSuccess);
            Assert.AreEqual(1, productsInCart.Count);
            Assert.AreSame(validProductToReserve, productsInCart[0]);
            Assert.AreEqual(reserveCount, store.Customer.Cart.GetProductCount(productsInCart[0]));
        }

        [TestMethod()]
        public void ReserveProducts_CountGreaterThanInventory_FailsAndDoesNotAddToCart()
        {
            // Arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product productToReserve = pair.Key;
            int reserveCount = pair.Value * 2;
            String msg;
            bool success;
            List<Product> productsInCart = null;

            // Act
            success = store.ReserveProducts(productToReserve, reserveCount, out msg);
            productsInCart = store.Customer.Cart.GetAllProducts();
            
            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual(0, productsInCart.Count);
        }

        [TestMethod()]
        public void ReserveProducts_MultipleReservationsWithTotalExceedingInventory_FailsAndHasCorrectMessage()
        {
            // Arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product productToReserve = pair.Key;
            int reserveCount1 = pair.Value;
            int reserveCount2 = 1;
            String msg;
            bool success1, success2;
            List<Product> productsInCart = null;

            // Act
            success1 = store.ReserveProducts(productToReserve, reserveCount1, out msg);
            success2 = store.ReserveProducts(productToReserve, reserveCount2, out msg);
            productsInCart = store.Customer.Cart.GetAllProducts();

            // Assert
            Assert.IsTrue(success1);
            Assert.IsFalse(success2);
            Assert.AreEqual(reserveCount1, store.Customer.Cart.GetProductCount(productToReserve));
            Assert.IsFalse(msg.ToLower().Contains("out of stock"));
        }

        [TestMethod()]
        public void ReserveProducts_OutOfStock_FailsAndHasCorrectMessage()
        {
            // Arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetOosReserveArguments();
            Product productToReserve = pair.Key;
            int reserveCount = 1;
            String msg;
            bool success;
            List<Product> productsInCart = null;

            // Act
            success = store.ReserveProducts(productToReserve, reserveCount, out msg);
            productsInCart = store.Customer.Cart.GetAllProducts();

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual(0, productsInCart.Count);
            Assert.IsTrue(msg.ToLower().Contains("out of stock"));
        }

        [TestMethod()]
        public void Checkout_WithValidParameters_SubtractsFromWallet()
        {
            // arrange variables
            Store store = CreateStore();
            User.Wallet wallet = store.Customer.Wallet;
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product productToReserve = pair.Key;
            int reserveCount = pair.Value;
            double totalPrice = productToReserve.Price * reserveCount;
            bool checkoutSuccess;
            double startingCash, endingCash;
            String msg;

            // act
            wallet.Pay(wallet.Cash - 0.000001);
            wallet.Fill(totalPrice);    // Ensure wallet has barely sufficient cash to cover the purchase
            startingCash = wallet.Cash;
            store.ReserveProducts(productToReserve, reserveCount, out msg); // Assume success, this is covered by other tests
            checkoutSuccess = store.Checkout(out msg);
            endingCash = wallet.Cash;

            // assert
            Assert.AreEqual(startingCash - totalPrice, endingCash, 0.001);
        }

        [TestMethod()]
        public void Checkout_WithValidParameters_SubtractsFromInventory()
        {
            // arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product productToReserve = pair.Key;
            int reserveCount1 = pair.Value;
            int reserveCount2 = 1;
            User.Wallet wallet = store.Customer.Wallet;
            double totalPrice = productToReserve.Price * reserveCount1;
            bool success1, success2, success3;
            String msg;

            // act
            wallet.Fill(totalPrice * 2);    // Ensure wallet has sufficient cash
            success1 = store.ReserveProducts(productToReserve, reserveCount1, out msg);
            success2 = store.Checkout(out msg);
            success3 = store.ReserveProducts(productToReserve, reserveCount2, out msg);

            // assert
            Assert.IsTrue(success1 && success2, "Could not complete preliminary reserve and checkout in order to test re-reserving");
            Assert.IsFalse(success3, "Re-reserve succeeded, should fail");
            Assert.IsTrue(msg.ToLower().Contains("out of stock"));
        }

        [TestMethod()]
        public void Checkout_WithValidParameters_ClearsCart()
        {
            // arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product productToReserve = pair.Key;
            int reserveCount = pair.Value / 2;
            User.Wallet wallet = store.Customer.Wallet;
            double totalPrice = productToReserve.Price * reserveCount;
            bool success1, success2;
            String msg;
            List<Product> productsInCart;

            // act
            wallet.Fill(totalPrice * 2);    // Ensure wallet has sufficient cash
            success1 = store.ReserveProducts(productToReserve, reserveCount, out msg);
            success2 = store.Checkout(out msg);
            productsInCart = store.Customer.Cart.GetAllProducts();

            // assert
            Assert.IsTrue(success1 && success2, "Could not complete preliminary reserve and checkout in order to test re-reserving");
            Assert.AreEqual(0, productsInCart.Count);
        }

        [TestMethod()]
        public void Checkout_GreaterThanWalletAmount_Fails()
        {
            // arrange variables
            Store store = CreateStore();
            KeyValuePair<Product, int> pair = GetValidReserveArguments(0);
            Product productToReserve = pair.Key;
            int reserveCount = pair.Value / 2;
            User.Wallet wallet = store.Customer.Wallet;
            double totalPrice = productToReserve.Price * reserveCount;
            bool success1, success2;
            String msg;

            // act
            if (wallet.CanPay(totalPrice))
            {
                // Remove cash from wallet, leaving $0.01 too little for the purchase
                double amountToSubtract = wallet.Cash - totalPrice + 0.01;
                wallet.Pay(amountToSubtract);
            }
            else if (wallet.Cash < totalPrice - 0.01)
            {
                // Add cash to wallet, but add $0.01 too little to cover purchase
                double amountToAdd = totalPrice - wallet.Cash - 0.01;
                wallet.Fill(amountToAdd);
            } // else leave wallet as-is, with just barely too little for the purchase
            success1 = store.ReserveProducts(productToReserve, reserveCount, out msg);
            success2 = store.Checkout(out msg);

            // assert
            Assert.IsTrue(success1, "Could not complete preliminary reserve in order to test checkout");
            Assert.IsFalse(success2);
        }





        private KeyValuePair<Product, int> GetValidReserveArguments(int index = 0)
        {
            KeyValuePair<Product, int> result = default(KeyValuePair<Product, int>);
            try
            {
                result = new KeyValuePair<Product, int>(_products[index], _counts[index]);
                if (result.Key == null || result.Value == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Assert.Fail(String.Format("Invalid test, could not get valid arguments for ReserveProducts with index {0}", index));
            }

            return result;
        }

        private KeyValuePair<Product, int> GetOosReserveArguments()
        {
            KeyValuePair<Product, int> result = default(KeyValuePair<Product, int>);
            try
            {
                int idx = 0;
                do
                {
                    if (_counts[idx] == 0)
                    {
                        result = new KeyValuePair<Product, int>(_products[idx], _counts[idx]);
                        break;
                    }
                } while (++idx > 0);    // Throws exception when nothing is found
            }
            catch (Exception)
            {
                Assert.Fail("Invalid test, could not find any out of stock items");
            }

            return result;
        }

        private void CreateInventoryFile()
        {
            
            using (var file = File.Open(INVENTORY_PATH, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    for (int i = 0; i < _products.Length; i++)
                    {
                        writer.WriteLine(String.Format("{0}\0{1}", _products[i].ToStorageString(), _counts[i]));
                    }
                }
            }
        }

        private void ReplaceProductListFromInventory(Inventory inv)
        {
            for (int i = 0; i < _products.Length; i++)
            {
                _products[i] = inv.GetProduct(_products[i].Id);
            }
        }

        private Store CreateStore()
        {
            Store result = new Store(INVENTORY_PATH);
            ReplaceProductListFromInventory(result.Inventory);
            return result;
        }

    }
}