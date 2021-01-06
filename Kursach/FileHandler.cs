using System;
using System.Globalization;
using System.IO;

namespace Kursach
{
    static class FileHandler
    {
        private static readonly string WorkingDirectory = Directory.GetCurrentDirectory();

        public static void LoadData(GroceryStore groceryStore)
        {
            if (!File.Exists(WorkingDirectory + @"\StoreData.txt"))
            {
                File.Create(WorkingDirectory + @"\StoreData.txt");
                return;
            }

            try
            {
                string[] depParams = new string[2];
                string[] prodParams = new string[5];
                using (var file = new StreamReader(WorkingDirectory + @"\StoreData.txt"))
                {
                    string line;
                    int index = 0;
                    while ((line = file.ReadLine()) != null)
                    {
			index = 0;
                        if (line.Equals(string.Empty)) return;
                        depParams = line.Split(';');
                        groceryStore.DepartmentList.Add(new Department(depParams[0]));
                        foreach (var element in depParams[1].Split('|'))
                        {
                            prodParams = element.Split(',');
                            groceryStore.DepartmentList[index].ProductList.Add(new Product(prodParams[0],
                                double.Parse(prodParams[1], CultureInfo.InvariantCulture),
                                double.Parse(prodParams[2], CultureInfo.InvariantCulture),
                                int.Parse(prodParams[3]), int.Parse(prodParams[4])));
                        }

                        ++index;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SaveData(GroceryStore groceryStore)
        {
            using (var file = new StreamWriter(WorkingDirectory + @"\StoreData.txt", false))
            {
                foreach (var dep in groceryStore.DepartmentList)
                {
                    file.Write(dep.Name + ";");
                    for (int i = 0; i < dep.ProductList.Count; ++i)
                    {
                        file.Write(dep.ProductList[i].Name + "," + dep.ProductList[i].Price
                                   + "," + dep.ProductList[i].PurchasePrice + "," + dep.ProductList[i].InStock
                                   + "," + dep.ProductList[i].Sold);
                        if (i != dep.ProductList.Count - 1) file.Write("|");
			else file.Write("\n")
                    }
                }
            }
        }
    }
}