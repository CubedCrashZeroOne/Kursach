using System;

namespace Kursach
{
    class Program
    {
        static void Main(string[] args)
        {
            GroceryStore groceryStore = new GroceryStore();
            FileHandler.LoadData(groceryStore);
            while (true)
            {
                MenuHandler.MenuOutput(groceryStore);
                Console.SetCursorPosition(0, 1);

                // Loop keeps waiting for key inputs until that input is enter.
                while (true)
                {
                    // Calls Console.ReadKey and writes it into KeyInputHandler.Key field
                    MenuHandler.GetKey();
                    if (MenuHandler.UpdateKey(groceryStore)) break;
                }
            }
        }
    }
}
