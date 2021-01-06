using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kursach
{
    class GroceryStore
    {
        public List<Department> DepartmentList = new List<Department>();
        private Department currentDepartment = null;

        /// <summary>
        /// Enter the department settings by index specified by the menu.
        /// </summary>
        /// <param name="index"></param>
        public void EnterDepartment(int index) => currentDepartment = DepartmentList[index];

        /// <summary>
        /// Exit the current department settings.
        /// </summary>
        public void ExitDepartment() => currentDepartment = null;

        /// <summary>
        /// Output products in the department for the menu.
        /// </summary>
        public void OutputDepartmentProducts()
        {
            foreach (var k in currentDepartment.ProductList)
            {
                Console.WriteLine("  " + k.Name);
            }
        }

        /// <summary>
        /// Returns the number of products in the current department.
        /// </summary>
        /// <returns></returns>
        public int CountProducts() => currentDepartment.ProductList.Count;

        /// <summary>
        /// Add a new Department to the store.
        /// </summary>
        /// <param name="name"></param>
        public void AddDepartment(string name)
        {
            foreach (var k in DepartmentList)
            {
                if (k.Name.Equals(name))
                {
                    Console.Clear();
                    Console.WriteLine("This department already exists.");
                    return;
                }
            }

            DepartmentList.Add(new Department(name));
            Console.Clear();
            Console.WriteLine("{0} was added to the store's list.", name);
        }

        /// <summary>
        /// Add a new product to the current department.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="purchasePrice"></param>
        public void AddProduct(string name, double price, double purchasePrice) =>
            currentDepartment.ProductList.Add(new Product(name, price, purchasePrice));

        public void EditProduct(int index, string name, double price) =>
            currentDepartment.ProductList[index].Edit(name, price);

        /// <summary>
        /// Remove a producct at a specified index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveProduct(int index) => currentDepartment.ProductList.RemoveAt(index);

        /// <summary>
        /// Sell a specific number of units from a specified product.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        public void SellProduct(int index, int amount) => currentDepartment.ProductList[index].Sell(amount);

        /// <summary>
        /// Restock on a specific number of units from a specified product.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        /// <param name="restockPrice"></param>
        public void RestockProduct(int index, int amount, out double restockPrice) =>
            currentDepartment.ProductList[index].Restock(amount, out restockPrice);

        /// <summary>
        /// Output the highest-grossing and lowest-grossing department info.
        /// </summary>
        public void HighestLowest()
        {
            var dep = DepartmentList.First(n => n.TotalIncome() == DepartmentList.Max(f => f.TotalIncome()));
            Console.Clear();
            Console.WriteLine("Highest-grossing department: {0}\nIncome: {1}", dep.Name, dep.TotalIncome());
            dep = DepartmentList.First(n => n.TotalIncome() == DepartmentList.Min(f => f.TotalIncome()));
            Console.WriteLine("Lowest-grossing department: {0}\nIncome: {1}", dep.Name, dep.TotalIncome());
        }

        /// <summary>
        /// Output the highest-grossing product.
        /// </summary>
        public void HighestProduct()
        {
            Product highest = null;
            double income = int.MinValue;
            foreach (var department in DepartmentList)
            {
                foreach (var product in department.ProductList)
                {
                    if (product.Income() > income)
                    {
                        highest = product;
                        income = product.Income();
                    }
                }
            }

            Console.Clear();
            if (highest != null)
            {
                highest.OutputInfo();
            }
            else
            {
                Console.WriteLine("this is an error.");
            }
        }

        /// <summary>
        /// Output all products in the current department.
        /// </summary>
        public void AllProducts()
        {
            Console.Clear();
            foreach (var k in currentDepartment.ProductList)
            {
                k.OutputInfo();
            }
        }

        /// <summary>
        /// View all prodducts that are out of stock.
        /// </summary>
        public void OutOfStock()
        {
            Console.Clear();
            Console.WriteLine("Out of stock products:");
            foreach (var k in currentDepartment.ProductList)
            {
                if (k.InStock == 0) Console.WriteLine(k.Name);
            }
        }

        /// <summary>
        /// Get total department income.
        /// </summary>
        public void TotalIncome()
        {
            Console.Clear();
            Console.WriteLine(currentDepartment.ProductList.Sum(n => n.Income()));
        }
    }
}