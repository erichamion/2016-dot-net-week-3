using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Store
{
    class Product
    {
        public uint Id { get; }
        public String Name { get; }
        public String Category { get; }
        public double Price { get; }

        private static uint nextId = 42272742;

        public enum ProductFields
        {
            ID, NAME, CATEGORY, PRICE
        }

        public Product(uint id, String name, String category, double price)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
        }

        public Product(String name, String category, double price) : this(nextId++, name, category, price) { }

        public Product(ref Queue<String> tokens)
        {
            if (tokens.Count < 4)
            {
                throw new FormatException(String.Format("Product string needs 4 tokens, found {0}", tokens.Count));
            }

            // Throws exception if the token doesn't represent an unsigned integer
            Id = uint.Parse(tokens.Dequeue());

            Name = tokens.Dequeue();

            Category = tokens.Dequeue();

            // Throws exception if the price doesn't look like a double
            Price = double.Parse(tokens.Dequeue());
        }

        public override string ToString()
        {
            return ToString("\t", ProductFields.ID, ProductFields.NAME, ProductFields.CATEGORY, ProductFields.PRICE);
        }

        public String ToString(String sep, params ProductFields[] fields)
        {
            String result = String.Empty;
            bool hasLooped = false;
            foreach (ProductFields field in fields)
            {
                String fieldString;
                switch (field)
                {
                    case ProductFields.ID:
                        fieldString = Id.ToString();
                        break;

                    case ProductFields.NAME:
                        fieldString = Name;
                        break;

                    case ProductFields.CATEGORY:
                        fieldString = Category;
                        break;

                    case ProductFields.PRICE:
                        fieldString = String.Format("{0:C}", Price);
                        break;

                    default:
                        fieldString = String.Empty;
                        break;
                }

                result = String.Format("{0}{1}{2}", result, (hasLooped ? sep : String.Empty), fieldString);
            }

            return result;
        }

        public String ToStorageString()
        {
            return ToString("\0", ProductFields.ID, ProductFields.NAME, ProductFields.CATEGORY, ProductFields.PRICE);
        }

    }
}
