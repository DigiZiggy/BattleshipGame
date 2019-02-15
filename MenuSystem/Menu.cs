using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuSystem
{
    public class Menu
    {
        public string Title { get; set; }

        public Dictionary<string, MenuItem> MenuItems { get; private set; } = new Dictionary<string, MenuItem>()
        {
            {
                "X", new MenuItem()
                {
                    Description = "Go back!",
                    Shortcut = "X",
                    MenuItemType = MenuItemType.GoBackOneLevel
                }
            },

            {
                "Q", new MenuItem()
                {
                    Description = "Quit to main menu!",
                    Shortcut = "Q",
                    MenuItemType = MenuItemType.GoBackToMain
                }
            }
        };

        public bool ClearScreenInMenuStart { get; set; } = true;

        public bool DisplayQuitToMainMenu { get; set; } = false;
        public bool GoBackOneLevel { get; set; } = false;
        public bool IsMainMenu { get; set; } = false;
        public bool IsGameMenu { get; set; } = false;
        public bool IsBoatsMenu { get; set; } = false;


        private KeyValuePair<string, MenuItem> goBackItem;
        private KeyValuePair<string, MenuItem> quitToMainItem;

        private void PrintMenu()
        {
            var defaultMenuChoice = MenuItems.FirstOrDefault(m => m.Value.IsDefaultChoice == true);

            if (ClearScreenInMenuStart)
            {
                Console.Clear();
            }

            Console.WriteLine("-------- " + Title + " --------\n");
            foreach (var dictionaryItem in MenuItems.Where(m => m.Value.MenuItemType == MenuItemType.Regular))
            {
                var menuItem = dictionaryItem.Value;

                if (menuItem.IsDefaultChoice)
                {
                    Console.ForegroundColor =
                        ConsoleColor.Blue;

                    // in place of ShortcutDescription we are using Keys now
                    Console.Write(dictionaryItem.Key);
                    Console.WriteLine(menuItem);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(dictionaryItem.Key);
                    Console.WriteLine(menuItem);
                }
            }

            Console.WriteLine("\n-----------------");


            if (goBackItem.Value != null)
            {
                Console.WriteLine(goBackItem.Key + goBackItem.Value + "\n");
            }

            if (DisplayQuitToMainMenu)
            {
                if (quitToMainItem.Value != null)
                {
                    Console.WriteLine(quitToMainItem.Key + quitToMainItem.Value);
                }
            }

            Console.Write(
                "[" + defaultMenuChoice.Key + "]>"
            );
        }

        private void WaitForUser()
        {
            Console.Write("Press any key to continue!");
            Console.ReadKey();
        }

        public void RunMenu()
        {
            goBackItem = MenuItems.FirstOrDefault(m => m.Value.MenuItemType == MenuItemType.GoBackOneLevel);
            quitToMainItem = MenuItems.FirstOrDefault(m => m.Value.MenuItemType == MenuItemType.GoBackToMain);

            var done = true;
            string input;
            do
            {
                done = false;

                PrintMenu();
                input = Console.ReadLine().ToUpper().Trim();

                var GoBackItem = MenuItems.FirstOrDefault(m => m.Value.MenuItemType == MenuItemType.GoBackOneLevel);

                if (input == goBackItem.Key)
                {
                    break;
                }

                MenuItem menuItem = null;

                menuItem = string.IsNullOrWhiteSpace(input)
                    ? null
                    : MenuItems.FirstOrDefault(item => item.Key == input).Value;


                if (menuItem == null)
                {
                    Console.WriteLine("Command not found!");
                    WaitForUser();
                    continue;
                }

                if (menuItem?.CommandToExecute == null)
                {
                    Console.WriteLine("This item has no action");
                    WaitForUser();
                    continue;
                }
                
                menuItem.CommandToExecute();
            } while (done != true);
        }
    }
}