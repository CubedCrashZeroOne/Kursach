using System;
using System.Collections.Generic;
using System.Linq;

namespace Kursach
{
    class Department
    {
        public List<Product> ProductList = new List<Product>();
        public string Name { get; }

        public Department(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Get total income based on products sold from this department.
        /// </summary>
        /// <returns></returns>
        public double TotalIncome()
        {
            double sum = 0;
            foreach (var k in ProductList)
            {
                sum += k.Income();
            }
            return sum;
        }

        /// <summary>
        /// Returns the highest-grossing product in the department.
        /// </summary>
        /// <returns></returns>
        public Product HighestProduct() =>
            ProductList.First(n => n.Income() == ProductList.Max(f => f.Income()));

    }
}
