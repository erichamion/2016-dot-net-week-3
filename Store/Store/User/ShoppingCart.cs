using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.User
{
    public class ShoppingCart
    {
        private Dictionary<Store.Product, int> _products = new Dictionary<Store.Product, int>();

        public ShoppingCart(Store.ICheckoutEventCreator checkoutCreator)
        {
            checkoutCreator.OnReserveEvent += AddProduct;
            checkoutCreator.OnPreReleaseEvent += BeforeSubtractProduct;
            checkoutCreator.OnReleaseEvent += SubtractProduct;
            checkoutCreator.OnTransactionEndedEvent += OnTransactionEnded;
        }

        private void AddProduct(Store.Product product, int count, int transactionId)
        {
            if (count == 0) return;

            if (count < 0)
            {
                throw new ArgumentException("Cannot add negative items to cart!");
            }

            int oldCount = GetProductCount(product);
            _products[product] = oldCount + count;
        }

        private void BeforeSubtractProduct(Store.Product product, int count, int transactionId, AddOnlyCollection<int> maxReleased)
        {
            int subtractable = DictionaryHelper.GetWithDefault(_products, product);
            if (subtractable < count)
            {
                maxReleased.Add(subtractable);
            }
        }

        private void SubtractProduct(Store.Product product, int count, int transactionId)
        {
            if (count == 0) return;

            if (count < 0)
            {
                throw new ArgumentException("Cannot subtract negative items from cart!");
            }
            
            // This key should exist because we checked in BeforeSubtractProduct
            int newCount = _products[product] - count;
            if (newCount <= 0)
            {
                // If removing all of the product, remove product from the cart entirely.
                RemoveProduct(product);
            }
            else
            {
                // Otherwise, change its count.
                _products[product] = newCount;
            }
        }

        private void OnTransactionEnded(int transactionId)
        {
            // The next transaction needs an empty cart.
            _products.Clear();
        }

        private void RemoveProduct(Store.Product product)
        {
            _products.Remove(product);
        }

        public List<Store.Product> GetAllProducts()
        {
            return new List<Store.Product>(_products.Keys);
        }

        public int GetProductCount(Store.Product product)
        {
            return DictionaryHelper.GetWithDefault(_products, product);
        }

        public double GetExtendedPrice(Store.Product product)
        {
            return product.Price * GetProductCount(product);
        }

        public double GetTotalPrice()
        {
            double result = 0;
            foreach (var pair in _products)
            {
                Store.Product product = pair.Key;
                int count = pair.Value;
                result += product.Price * count;
            }
            return result;
        }
    }
}
