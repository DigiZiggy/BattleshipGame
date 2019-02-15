using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using GamePlay;
using Initializers;


namespace MenuSystem
{
    public class ApplicationMenu
    {
        public Menu GameMenu;
        public Menu MainMenu;
        public Menu BoatsMenu;
        public Menu ConfigMenu;
        private Board _board;
        private Player _player;
        private GameUI _gameUi;
        private GameLogic _gameLogic;
        private Game loadedGame;
        private Initialise _dbInitialise = new Initialise();

        public void RunMenu()
        {
            BoatsMenu = new Menu()
            {
                Title = "Place Boats",
                IsBoatsMenu = true,
                GoBackOneLevel = true,
                DisplayQuitToMainMenu = false,
            
            };
            BoatsMenu.MenuItems.Add("P", new MenuItem()
            {
                Description = "Add Patrol (length of 1)",
                Shortcut = "P",
                CommandToExecute = UserAddPatrol,
                IsDefaultChoice = true
            });
            BoatsMenu.MenuItems.Add("CR", new MenuItem()
            {
                Description = "Add Cruiser (length of 2)",
                Shortcut = "CR",
                CommandToExecute = UserAddCruiser,
                IsDefaultChoice = false
            });    
            BoatsMenu.MenuItems.Add("S", new MenuItem()
            {
                Description = "Add Submarine (length of 3)",
                Shortcut = "S",
                CommandToExecute = UserAddSubmarine,
                IsDefaultChoice = false
            });
            BoatsMenu.MenuItems.Add("B", new MenuItem()
            {
                Description = "Add Battleship (length of 4)",
                Shortcut = "B",
                CommandToExecute = UserAddBattleship,
                IsDefaultChoice = false
            });  
            BoatsMenu.MenuItems.Add("CA", new MenuItem()
            {
                Description = "Add Carrier (length of 5)",
                Shortcut = "CA",
                CommandToExecute = UserAddCarrier,
                IsDefaultChoice = false
            }); 
            BoatsMenu.MenuItems.Add("G", new MenuItem()
            {
                Description = "Start Game",
                Shortcut = "G",
                CommandToExecute = StartCustomGamePlay,
                IsDefaultChoice = false
            }); 

            GameMenu = new Menu()
            {
                Title = "Game",
                IsGameMenu = true,
                GoBackOneLevel = true,
                DisplayQuitToMainMenu = false,
            };
            GameMenu.MenuItems.Add(
                "G", new MenuItem()
                {
                    Description = "Generate board for me",
                    Shortcut = "G",
                    CommandToExecute = StartRandomGame,
                    IsDefaultChoice = true
                }
            );
            GameMenu.MenuItems.Add(
                "C", new MenuItem()
                {
                    Description = "Create my own board",
                    Shortcut = "C",
                    CommandToExecute = StartCustom,
                    IsDefaultChoice = false
                }
            );
            GameMenu.MenuItems.Add(
                "L", new MenuItem()
            {
                Description = "Load Game",
                Shortcut = "L",
                CommandToExecute = LoadGame,
                IsDefaultChoice = false
            });
            
            ConfigMenu = new Menu()
            {
                Title = "Configuration",
                GoBackOneLevel = true,
                DisplayQuitToMainMenu = false,
            };
            ConfigMenu.MenuItems.Add("B", new MenuItem()
            {
                Shortcut = "B",
                MenuItemType = MenuItemType.Regular,
                CommandToExecute = ChangeBoardSize,
                Description = "Board - change board size"
            });
            
            ConfigMenu.MenuItems.Add("C", new MenuItem()
            {
                Shortcut = "C",
                MenuItemType = MenuItemType.Regular,
                CommandToExecute = ChangeShipsCanTouch,
                Description = "Can ships touch?"
            });
            
            ConfigMenu.MenuItems.Add("S", new MenuItem()
            {
                Shortcut = "S",
                MenuItemType = MenuItemType.Regular,
                CommandToExecute = ChangeShips,
                Description = "Ships - change the amount & sizes"
            });
                        
            MainMenu = new Menu()
            {
                Title = "Main menu",
                IsMainMenu = true,
            };
            MainMenu.MenuItems.Add(
                "N", new MenuItem()
                {
                    Description = "New game",
                    Shortcut = "N",
                    CommandToExecute = StartGame,
                    IsDefaultChoice = true
                }
            );
            MainMenu.MenuItems.Add("L", new MenuItem()
            {
                Description = "Load Game",
                Shortcut = "L",
                CommandToExecute = LoadGame,
                IsDefaultChoice = false
                }
            );
            MainMenu.MenuItems.Add("S", new MenuItem()
            {
                Shortcut = "S",
                MenuItemType = MenuItemType.Regular,
                CommandToExecute = ConfigMenu.RunMenu,
                Description = "Settings"
            });
            
            MainMenu.RunMenu();
        }
        
        private void StartGame()
        {
            Console.WriteLine("Please enter your name: ");
            var player = Console.ReadLine();
            _player = new Player(player);
            _board = new Board(_player);
            _gameUi = new GameUI(_player, _board);
            _gameLogic = new GameLogic(_player, _board);
            GameMenu.RunMenu();
        }

        private void StartCustom()
        {
            _gameLogic.ClearBoard(_board.EnemyBoard);
            _gameLogic.ClearBoard(_board.PlayerBoard);
            BoatsMenu.RunMenu();
        }
                
        private void StartRandomGame()
        {
            Console.Clear();
            _gameLogic.ClearBoard(_board.EnemyBoard);
            _gameLogic.ClearBoard(_board.PlayerBoard);
            _board = new Board(_player);
            _gameUi = new GameUI(_player, _board);
            _gameLogic = new GameLogic(_player, _board);
            _gameLogic.RandomizeBoatsOnBoard(_board.EnemyBoard, Game.CanShipsTouch, Game.ShipsCountAndSizes);
            _gameLogic.RandomizeBoatsOnBoard(_board.PlayerBoard, Game.CanShipsTouch, Game.ShipsCountAndSizes);
            Console.WriteLine(_gameUi.GetBoardString());
            _gameLogic.PlayGame();
        }
        
        private void StartCustomGamePlay()
        {
            Console.Clear();
            _gameLogic.RandomizeBoatsOnBoard(_board.EnemyBoard, Game.CanShipsTouch, Game.ShipsCountAndSizes);
            Console.WriteLine(_gameUi.GetBoardString());
            _gameLogic.PlayGame();
        }
        
        public void LoadGame()
        {
            Console.Clear();
            loadedGame = Initialise.LoadAndDeserializeGame();
            _player = loadedGame.Player;
            _board = loadedGame.Board;
            _gameUi = new GameUI(_player, _board);
            _gameLogic = new GameLogic(_player, _board);
            Console.WriteLine(_gameUi.GetBoardString());
            Console.WriteLine("\nAny time you wish to Save the game, type in SAVE");
            Console.WriteLine("or EXIT to Exit Game.");
            _gameLogic.PlayGame();

        }

        private void UserAddPatrol()
        {
            Console.Clear();
            Console.WriteLine(_gameUi.GetBoardString());
            _gameLogic.UserAddBoat(1);
        }
        
        private void UserAddCruiser()
        {
            Console.Clear();
            Console.WriteLine(_gameUi.GetBoardString());
            _gameLogic.UserAddBoat(2);
        }
        
        private void UserAddSubmarine()
        {
            Console.Clear();
            Console.WriteLine(_gameUi.GetBoardString());
            _gameLogic.UserAddBoat(3);
        }
        
        private void UserAddBattleship()
        {
            Console.Clear();
            Console.WriteLine(_gameUi.GetBoardString());
            _gameLogic.UserAddBoat(4);
        }
        
        private void UserAddCarrier()
        {
            Console.Clear();
            Console.WriteLine(_gameUi.GetBoardString());
            _gameLogic.UserAddBoat(5);
        }
        
        private void ChangeShips()
        {
            Console.WriteLine("Current ships: ");
            Game.ShipSizeToString();
            Console.WriteLine("Insert comma separated values for ship sizes");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("You did not change anything. Press enter to continue");
                Console.ReadLine();
                return;
            };
            var newShips = input.Split(',').Select(int.Parse).ToList();
            foreach (var ship in newShips)
            {
                if (ship > 5)
                {
                    Console.WriteLine("Biggest ship can be Carrier with length of 5. Press enter to continue");
                    Console.ReadLine();
                    return;
                }
            }
            Game.ShipsCountAndSizes = newShips;
        }
        
        private void ChangeShipsCanTouch()
        {
            Console.WriteLine($"Currently ships can touch setting: {Game.CanShipsTouch}");
            Console.WriteLine("Press enter to change it");
            var input = Console.ReadLine();
            Game.CanShipsTouch = !Game.CanShipsTouch;
        }
        
        
        private void ChangeBoardSize()
        {
            Console.WriteLine($"Current boardSize: {Game.BoardSize}");
            Console.WriteLine("Choose your board size: ");
            var input = Console.ReadLine();
            Game.BoardSize = int.Parse(input);
        }
    }
}
