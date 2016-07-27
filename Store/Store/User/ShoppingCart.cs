using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.User
{
    class ShoppingCart
    {
        private Dictionary<Store.Product, int> _products = new Dictionary<Store.Product, int>();

        public void AddProduct(Store.Product product, int count)
        {
            if (count < 1)
            {
                throw new ArgumentException("Cannot add 0 or negative items to cart!");
            }

            int oldCount = GetProductCount(product);
            _products[product] = oldCount + count;
        }

        public void SubtractProduct(Store.Product product, int count)
        {
            if (count < 1)
            {
                throw new ArgumentException("Cannot subtract 0 or negative items from cart!");
            }
            if (_products.ContainsKey(product))
            {
                throw new ArgumentException("Attempted to subtract from an item that is not in the cart!");
            }

            int newCount = _products[product] - count;
            if (newCount <= 0)
            {
                // If removing all of the product, remove product from the cart entirely.
                _products.Remove(product);
            }
            else
            {
                // Otherwise, change its count.
                RemoveProduct(product);
            }
        }

        public void RemoveProduct(Store.Product product)
        {
            _products.Remove(product);
        }

        public void Clear()
        {
            _products.Clear();
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
