using System;
using System.Globalization;

namespace Kursach
{
    class Product
    {

        public string Name { get; set; }
        public double Price { get; set; }
        public double PurchasePrice { get; private set; }
        public int InStock { get; set; }
        public int Sold { get; set; }

        public Product(string name, double price, double purchasePrice)
        {
            Name = name;
            Price = price;
            PurchasePrice = purchasePrice;
            InStock = 0;
            Sold = 0;
        }

        public Product(string name, double price, double purchasePrice, int inStock, int sold)
        {
            Name = name;
            Price = price;
            PurchasePrice = purchasePrice;
            InStock = inStock;
            Sold = sold;
        }

        /// <summary>
        /// Calculate total income from all units sold.
        /// </summary>
        /// <returns></returns>
        public double Income() => Price * Sold;

        /// <summary>
        /// Replace name and price with new values.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        public void Edit(string name, double price)
        {
            Name = name;
            Price = price;
        }

        /// <summary>
        /// Sell a specific number of units.
        /// </summary>
        /// <param name="amount"></param>
        public void Sell(int amount)
        {
            if (amount <= InStock)
            {
                InStock -= amount;
                Sold += amount;
                Console.Clear();
                Console.WriteLine("Sold {0} units", amount);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Couldn't sell more than is currently in stock.");
            }
        }

        /// <summary>
        /// Restock on a specific number of units.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="restockPrice"></param>
        public void Restock(int amount, out double restockPrice)
        {
            restockPrice = amount * PurchasePrice;
            InStock += amount;
        }

        public void OutputInfo() =>
            Console.WriteLine("Name: {0}\nPrice: {1}\nIn stock: {2}\nSold: {3}\n", Name, Price, InStock, Sold);

        
    }
}
