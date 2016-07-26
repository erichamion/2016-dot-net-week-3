using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Store
{
    class Inventory
    {
        private Dictionary<Product, int> _productCounts = new Dictionary<Product, int>();
        private AccessLevels _accessLevel;

        private delegate String ShowProductDelegate(Product product);
        private ShowProductDelegate _ShowProduct;

        public AccessLevels AccessLevel
        {
            get { return _accessLevel; }
            set
            {
                switch (value)
                {
                    case AccessLevels.USER:
                        _ShowProduct = new ShowProductDelegate(ShowProductUser);
                        break;

                    case AccessLevels.ADMIN:
                        _ShowProduct = new ShowProductDelegate(ShowProductAdmin);
                        break;
                }
                _accessLevel = value;
            }
        }

        public enum AccessLevels
        {
            USER, ADMIN
        }

        public Inventory(AccessLevels accessLevel)
        {
            AccessLevel = accessLevel;
        }

        public void AddProduct(Product product, int count = 1)
        {
            int oldCount = GetProductCount(product);
            _productCounts[product] = oldCount + count;
        }

        public bool CheckAvailability(uint productId, int count = 1)
        {
            return CheckAvailability(GetProduct(productId), count);
        }

        public bool CheckAvailability(Product product, int count = 1)
        {
            return GetProductCount(product) >= count;
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

        public String ShowProduct(Product product)
        {
            return _ShowProduct(product);
        }

        public String ShowProduct(uint productId)
        {
            Product product = GetProduct(productId);
            return (product != null) ? ShowProduct(product) : String.Empty;
        }

        private int GetProductCount(Product product)
        {
            int count = 0;
            bool hasValue = (product != null) ? _productCounts.TryGetValue(product, out count) : false;
            if (!hasValue)
            {
                count = 0;
            }

            return count;
        }

        private String ShowProductUser(Product product)
        {
            String availability = CheckAvailability(product) ? "Yes" : "No";
            return String.Format("{0}\t{1}", product.ToString(), availability);
        }

        private String ShowProductAdmin(Product product)
        {
            return String.Format("{0}\t{1}", product.ToString(), _productCounts[product]);
        }
    }
}
