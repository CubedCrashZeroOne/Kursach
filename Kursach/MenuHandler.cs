using System;
using System.Globalization;

namespace Kursach
{
    enum MenuState
    {
        Main,
        ChooseDep,
        DepSettings,
        EditProd,
        RemoveProd,
        SellProd,
        RestockProd
    }

    static class MenuHandler
    {
        private static ConsoleKey Key;
        private static MenuState State = MenuState.Main;
        private static int Choice = 1;
        private static bool cancel = false;

        public static void GetKey()
        {
            // 
            Key = Console.ReadKey(true).Key;
        }

        public static bool UpdateKey(GroceryStore groceryStore)
        {
            // 
            int cap = State == MenuState.Main ? 6 :
                State == MenuState.ChooseDep ? groceryStore.DepartmentList.Count + 1 :
                State == MenuState.DepSettings ? 9 : groceryStore.CountProducts() + 1;

            switch (Key)
            {
                case ConsoleKey.UpArrow:
                    MoveUp();
                    return false;
                case ConsoleKey.DownArrow:
                    MoveDown(cap);
                    return false;
                case ConsoleKey.Enter:
                    Menu(groceryStore);
                    return true;
                default:
                    Console.SetCursorPosition(0, Choice);
                    return false;
            }
        }

        private static void MoveUp()
        {
            // decrement by 1, cap at 1, move cursor up
            Choice = Choice > 1 ? --Choice : 1;
            Console.SetCursorPosition(0, Choice);
        }

        private static void MoveDown(int cap)
        {
            // increment by 1, cap at cap, move cursor down
            Choice = Choice < cap ? ++Choice : cap;
            Console.SetCursorPosition(0, Choice);
        }

        public static void MenuOutput(GroceryStore groceryStore)
        {
            switch (State)
            {
                case MenuState.Main:
                    Console.WriteLine("    -Main Menu-\n  Add Department\n  Department Settings\n  View highest/lowest-grossing departments\n  See highest-grossing product\n  Save\n  Quit\n");
                    break;
                case MenuState.ChooseDep:
                    Console.WriteLine("    -Choose Department-");
                    foreach (var k in groceryStore.DepartmentList)
                    {
                        Console.WriteLine("  " + k.Name);
                    }

                    Console.WriteLine("  Cancel");
                    break;
                case MenuState.DepSettings:
                    Console.WriteLine(
                        "    -Department Settings-\n  Add product\n  Edit product\n  Remove product\n  Restock on product\n  Sell product\n  View all products\n  View out-of-stock products\n  View total department income\n  Cancel");
                    break;
                case MenuState.EditProd:
                case MenuState.RemoveProd:
                case MenuState.RestockProd:
                case MenuState.SellProd:
                    Console.WriteLine("    -Choose a Product-");
                    groceryStore.OutputDepartmentProducts();
                    Console.WriteLine("  Cancel");
                    break;
            }
        }

        private static void Menu(GroceryStore groceryStore)
        {
            string name;
            double price, purchasePrice;
            int amount;
            switch (State)
            {
                case MenuState.Main:
                    switch (Choice)
                    {
                        // Add department. 
                        case 1:
                            try
                            {
                                Console.Clear();
                                Console.WriteLine("Department Name:");
                                name = Console.ReadLine();
                                if (name.Contains(";") || name.Contains("|") || name.Contains(","))
                                {
                                    Console.Clear();
                                    Console.WriteLine("names can't contain \',\', \';\' or \'|\'");
                                    break;
                                }

                                groceryStore.AddDepartment(name);
                                break;
                            }
                            catch (NullReferenceException)
                            {
                                Console.WriteLine("This field can't be left empty.");
                                break;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Error.");
                                break;
                            }
                        // Enter Department settings.
                        case 2:
                            State = MenuState.ChooseDep;
                            Console.Clear();
                            MenuOutput(groceryStore);
                            Choice = 1;
                            Console.SetCursorPosition(0, 1);
                            while (true)
                            {
                                GetKey();
                                if (UpdateKey(groceryStore)) break;
                            }
                            Choice = 1;
                            Console.Clear();
                            Console.WriteLine("Entered department settings.");
                            break;
                        // Highest/Lowest-grossing departments
                        case 3:
                            groceryStore.HighestLowest();
                            break;
                        // Highest-grossing product
                        case 4:
                            groceryStore.HighestProduct();
                            break;
                        // Save data
                        case 5:
                            FileHandler.SaveData(groceryStore);
                            Console.Clear();
                            Console.WriteLine("Saved successfully.");
                            break;
                        // Quit
                        case 6:
                            Console.WriteLine("Do you wish to save your progress? y/n");
                            char choiceYesNo;
                            try
                            {
                                choiceYesNo = Console.ReadLine()[0];
                            }
                            catch (Exception)
                            {
                                choiceYesNo = ' ';
                            }

                            if (choiceYesNo == 'y')
                            {
                                FileHandler.SaveData(groceryStore);
                            }
                            else if (choiceYesNo != 'n')
                            {
                                Console.WriteLine("That is not a valid option.");
                                break;
                            }

                            Environment.Exit(0);
                            break;
                    }

                    if (!cancel) Console.ReadKey();
                    cancel = false;
                    Choice = 1;
                    Console.Clear();
                    break;
                case MenuState.ChooseDep:
                    // If cancel is pressed.
                    if (Choice == groceryStore.DepartmentList.Count + 1)
                    {
                        cancel = true;
                        State = MenuState.Main;
                        return;
                    }

                    groceryStore.EnterDepartment(Choice - 1);
                    State = MenuState.DepSettings;
                    Choice = 1;
                    Console.Clear();
                    break;
                case MenuState.DepSettings:
                    switch (Choice)
                    {
                        // Add product.
                        case 1:
                            try
                            {
                                Console.Clear();
                                Console.WriteLine("Product name:");
                                name = Console.ReadLine();
                                if (name.Contains(";") || name.Contains("|") || name.Contains(","))
                                {
                                    Console.Clear();
                                    Console.WriteLine("names can't contain \',\', \';\' or \'|\'");
                                    break;
                                }

                                Console.WriteLine("Price:");
                                price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                                Console.WriteLine("Purchase price:");
                                purchasePrice = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                                groceryStore.AddProduct(name, price, purchasePrice);
                                Console.Clear();
                                Console.WriteLine("{0} was added to the department's list.", name);
                            }
                            catch (NullReferenceException)
                            {
                                Console.WriteLine("This field can't be left empty.");
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Please, input a number.");
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Error.");
                            }

                            break;
                        // Edit product
                        case 2:
                            State = MenuState.EditProd;
                            Console.Clear();
                            MenuOutput(groceryStore);
                            Choice = 1;
                            Console.SetCursorPosition(0, 1);
                            while (true)
                            {
                                GetKey();
                                if (UpdateKey(groceryStore)) break;
                            }

                            State = MenuState.DepSettings;
                            break;
                        // Remove product
                        case 3:
                            State = MenuState.RemoveProd;
                            Console.Clear();
                            MenuOutput(groceryStore);
                            Choice = 1;
                            Console.SetCursorPosition(0, 1);
                            while (true)
                            {
                                GetKey();
                                if (UpdateKey(groceryStore)) break;
                            }

                            State = MenuState.DepSettings;
                            break;
                        // Restock on product
                        case 4:
                            State = MenuState.RestockProd;
                            Console.Clear();
                            MenuOutput(groceryStore);
                            Choice = 1;
                            Console.SetCursorPosition(0, 1);
                            while (true)
                            {
                                GetKey();
                                if (UpdateKey(groceryStore)) break;
                            }

                            State = MenuState.DepSettings;
                            break;
                        // Sell product
                        case 5:
                            State = MenuState.SellProd;
                            Console.Clear();
                            MenuOutput(groceryStore);
                            Choice = 1;
                            Console.SetCursorPosition(0, 1);
                            while (true)
                            {
                                GetKey();
                                if (UpdateKey(groceryStore)) break;
                            }

                            State = MenuState.DepSettings;
                            break;
                        // View all
                        case 6:
                            groceryStore.AllProducts();
                            break;
                        // View out-of-stock
                        case 7:
                            groceryStore.OutOfStock();
                            break;
                        // Total income
                        case 8:
                            groceryStore.TotalIncome();
                            break;
                        // Cancel
                        case 9:
                            groceryStore.ExitDepartment();
                            cancel = true;
                            State = MenuState.Main;
                            break;
                    }

                    if (!cancel) Console.ReadKey();
                    cancel = false;
                    Choice = 1;
                    Console.Clear();
                    break;
                case MenuState.EditProd:
                    // If cancel is pressed
                    if (Choice == groceryStore.CountProducts() + 1)
                    {
                        State = MenuState.DepSettings;
                        cancel = true;
                        return;
                    }

                    try
                    {
                        Console.Clear();
                        Console.WriteLine("Enter product name:");
                        name = Console.ReadLine();
                        Console.WriteLine("Enter price:");
                        price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                        groceryStore.EditProduct(Choice - 1, name, price);
                        Choice = 1;
                        Console.Clear();
                        Console.WriteLine("Changed to {0}", name);
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        Console.WriteLine("Please, enter a valid number.");
                    }

                    State = MenuState.DepSettings;
                    Choice = 1;
                    Console.Clear();
                    break;
                case MenuState.RemoveProd:
                    // If cancel is pressed
                    if (Choice == groceryStore.CountProducts() + 1)
                    {
                        State = MenuState.DepSettings;
                        cancel = true;
                        return;
                    }

                    groceryStore.RemoveProduct(Choice - 1);
                    State = MenuState.DepSettings;
                    Choice = 1;
                    Console.Clear();
                    Console.WriteLine("Product removed.");
                    break;
                case MenuState.SellProd:
                    // If cancel is pressed
                    if (Choice == groceryStore.CountProducts() + 1)
                    {
                        State = MenuState.DepSettings;
                        cancel = true;
                        return;
                    }

                    try
                    {
                        Console.Clear();
                        Console.WriteLine("How many units to sell:");
                        amount = int.Parse(Console.ReadLine());
                        groceryStore.SellProduct(Choice - 1, amount);
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        Console.WriteLine("Please, enter a valid integer.");
                    }

                    State = MenuState.DepSettings;
                    Choice = 1;
                    break;
                case MenuState.RestockProd:
                    // If cancel is pressed
                    if (Choice == groceryStore.CountProducts() + 1)
                    {
                        State = MenuState.DepSettings;
                        cancel = true;
                        return;
                    }

                    try
                    {
                        Console.Clear();
                        Console.WriteLine("How many units to buy:");
                        amount = int.Parse(Console.ReadLine());
                        groceryStore.RestockProduct(Choice - 1, amount, out double restockPrice);
                        Console.Clear();
                        Console.WriteLine("{0} unit(s) bought for {1}", amount, restockPrice);
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        Console.WriteLine("Please, enter a valid integer.");
                    }

                    State = MenuState.DepSettings;
                    Choice = 1;
                    break;
            }
        }
    }
}