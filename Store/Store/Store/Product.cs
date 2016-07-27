using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
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

        /**
         * intro is printed before everything else. lineIntro is printed
         * before every line *except the first line*. If outro is not
         * empty and is not null, then the lineIntro and outro will be
         * printed on a new line.
         */
        public String PrettyPrint(String intro, String lineIntro, String outro = "")
        {
            int labelSize = "Category: ".Length;
            String labelFormatter = String.Format("{{0,-{0}}}", labelSize);
            String[] labelNames = new String[] { "Id:", "Name:", "Category", "Price:" };
            String[] labels = new String[labelNames.Length];
            for (int i = 0; i < labelNames.Length; i++)
            {
                labels[i] = String.Format(labelFormatter, labelNames[i]);
            }

            String effectiveIntro = intro ?? String.Empty;
            String effectiveOtherLineIntro = lineIntro ?? String.Empty;
            String result = String.Format("{0}{6}{1}\n{2}{7}{3}\n{2}{8}{4}\n{2}{9}{5:C}",
                effectiveIntro, Id, effectiveOtherLineIntro, Name, Category, Price,
                labels[0], labels[1], labels[2], labels[3]);
            if (!String.IsNullOrEmpty(outro))
            {
                result = String.Format("{0}\n{1}{2}", result, lineIntro, outro);
            }
            return result;
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
                hasLooped = true;
            }

            return result;
        }

        public String ToStorageString()
        {
            return ToString("\0", ProductFields.ID, ProductFields.NAME, ProductFields.CATEGORY, ProductFields.PRICE);
        }

    }
}
