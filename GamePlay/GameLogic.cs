using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Domain;
using Initializers;

namespace GamePlay
{
    public class GameLogic
    {
        private readonly int BoardRows = Game.BoardSize;
        public int BoardCols = Game.BoardSize;
        private Player Player;
        public GameUI GameUi;
        public Board Board;
        public static readonly Random GetRandom = new Random();
        public List<List<BoardSquareState>> EnemyBoard;
        public List<List<BoardSquareState>> PlayerBoard;
        public List<List<BoardSquareState>> BoardCopy = new List<List<BoardSquareState>>();
        public List<List<BoardSquareState>> OpponentBoardCopy = new List<List<BoardSquareState>>();
        private readonly List<Ship> _ships = new List<Ship>();
        public Game savedGame;
        
        
        public Dictionary<string, int> YcoordinatesChar = new Dictionary<string, int>()
        {
            {"A", 1}, {"B", 2}, {"C", 3}, {"D", 4}, {"E", 5},
            {"F", 6}, {"G", 7}, {"H", 8}, {"I", 9}, {"J", 10},
            {"K", 11}, {"L", 12}, {"M", 13}, {"N", 14}, {"O", 15},
            {"P", 16}, {"Q", 17}, {"R", 18}, {"S", 19}, {"T", 20}
        };
        
        public GameLogic()
        {

        }        
        public GameLogic(Player player, Board board)
        {
            Player = player;
            Board = board;
            EnemyBoard = board.EnemyBoard;
            PlayerBoard = board.PlayerBoard;
            GameUi = new GameUI(player, board);
        }
        
        public void MarkHit(List<List<BoardSquareState>> board, int row, int col)
        {
            board[row][col].Hit = true;
        }
        
        public void MarkMiss(List<List<BoardSquareState>> board, int row, int col)
        {
            board[row][col].Miss = true;
        }

        private void GetUserInputAndPlaceBoat(int boatLength)
        {
            bool isXgood = true;
            String getX;
            do
            {
                Console.WriteLine("Enter x coordinate letter:");
                String xCoordinate = Console.ReadLine()?.ToUpper();
                getX = xCoordinate;
                isXgood = ValidateUserXcoordinate(xCoordinate);
                    
            } while (isXgood);
            
            var x = YcoordinatesChar[getX];
                
            bool isYgood = true;
            String getY;
            do
            {
                Console.WriteLine("Enter y coordinate letter:");
                String yCoordinate = Console.ReadLine()?.ToUpper();
                getY = yCoordinate;
                isYgood = ValidateUserYcoordinate(yCoordinate);
                    
            } while (isYgood);
                          
            int y = int.Parse(getY);
            
        
            bool isDirection = true;
            String getDir;
            do
            {
                Console.WriteLine("Enter direction (V for vertical or H for horizontal):");
                String direction = Console.ReadLine().ToUpper();
                getDir = direction;
                isDirection = ValidateDirection(direction);
                    
            } while (isDirection);
            
        
            Console.WriteLine("Entered x and y coordinates are : " + x + " and " + y + 
                              ",with direction of " + getDir);
            try
            {
                Console.WriteLine("Y koord " + y + "X koord" + x);
                PlaceCustomGameBoat(PlayerBoard, Game.CanShipsTouch, y-1, x-1, boatLength, getDir);
                Console.Clear();
                Console.WriteLine(GameUi.GetBoardString());
        
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Clear();
                Console.WriteLine(GameUi.GetBoardString());
                Console.WriteLine("\n");
                Console.WriteLine("<------- You were trying to place boat out of bounds! Try again. ------->");
                WaitForUser();
            }
            catch (ConstraintException)
            {
                Console.Clear();
                Console.WriteLine(GameUi.GetBoardString());
                Console.WriteLine("\n");
                Console.WriteLine("<------- There is a boat there already! Try again. ------->");
                WaitForUser();
            }
        }
        
        public bool ValidateDirection(String direction)
        {
            int tryGetDir;
            if (int.TryParse(direction, out tryGetDir))
            {
                Console.WriteLine("<------- Wrong input! Try again! Enter a letter V or H, not number! ------->");
                return true;
            }

            if (direction == "H" || direction == "V")
            {
                return false;
            }
            Console.WriteLine("<------- Wrong input! Try again! Enter a letter V or H! ------->");
            return true;            
        }
        
        
        public bool ValidateUserYcoordinate(String coordinate)
        {
            if (coordinate == "EXIT")
            {
                return false;
            }
            if (coordinate == "SAVE")
            {
                SaveGame();
                return true;
            }
            
            int tryGetY;
            if (int.TryParse(coordinate, out tryGetY))
            {
                if (tryGetY < 0 || tryGetY > BoardCols)
                {
                    Console.WriteLine("<------- Wrong input! Try again! Enter a number that would fit to board ------->");
                    return true;
                }
                   
                return false;
            }
            Console.WriteLine("<------- Wrong input! Try again! Enter a number not letter! ------->");
            return true;
        }

        public void SaveGame()
        {   
            savedGame = new Game(Game.BoardSize, Game.CanShipsTouch, Game.ShipsCountAndSizes, Player, Board);
            Initialise.SerializeGameAndSave(savedGame);
            Console.WriteLine("<------- GAME SAVED ------->");
        }
        
        public void ClearBoard(List<List<BoardSquareState>> board)
        {
            for (int i = 0; i < BoardRows; i++)
            {

                for (int j = 0; j < BoardCols; j++)
                {

                    board[i][j].Ship = false;
                    board[i][j].isEnemyShip = false;
                    board[i][j].Visited = false;
                    board[i][j].Hit = false;
                    board[i][j].Miss = false;
                    board[i][j].boatCannotTouch = false;
                }
            }
        }
        
        public bool ValidateUserXcoordinate(String coordinate)
        {
            
            if (coordinate == "EXIT")
            {
                return false;
            }
            if (coordinate == "SAVE")
            {
                SaveGame();
                return true;
            }           
            int tryGetX;
            if (int.TryParse(coordinate, out tryGetX))
            {
                Console.WriteLine("<------- Wrong input! Try again! Enter a letter ------->");
                return true;
            }
            try
            {
                var x = YcoordinatesChar[coordinate];
                if (x > BoardRows)
                {
                    Console.WriteLine("<------- Wrong input! Try again! Enter a letter from A to J ------->");
                    return true;
                }
                return false;
                
            }catch (KeyNotFoundException)
            {
                Console.WriteLine("<------- Wrong input! Try again! Enter a letter from A to J ------->");
                return true;
            }catch (FormatException)
            {
                Console.WriteLine("<------- Wrong input! Try again! Enter a letter from A to J ------->");
                return true;
            }
        }
        
        
        public void PlaceCustomGameBoat(List<List<BoardSquareState>> board, bool canBoatsTouch, int row, int col, int length, String direction)
        {
            if (length == 1)
                _ships.Add(new Ship.Patrol());
            if (length == 2)
                _ships.Add(new Ship.Cruiser());
            if (length == 3)
                _ships.Add(new Ship.Submarine());
            if (length == 4)
                _ships.Add(new Ship.Battleship());
            if (length == 5)
                _ships.Add(new Ship.Carrier());
            
            for (int i = 0; i < 1; i++)
            {
                if (direction.ToUpper() == "H")
                {             
                    int count = 0;
                    if (TryVertically(board, canBoatsTouch, row, col+length, length))
                    {
                        for (int j = 0; j < length; j++)
                        {
                            try
                            {
                                AddBoat(board, row, col + j);
                                if (canBoatsTouch == false)
                                {
                                    AddBoatSurrounding(board, row, col + j);

                                }
                                count++;
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                for (int k = 0; k < count; k++)
                                {
                                    DeleteBoat(row, col + k);
                                }
                                throw new ArgumentOutOfRangeException();
                                //code specifically for a ArgumentNullException
                            }
                        }
                    }
                    else
                    {
                        throw new ConstraintException();
                    }
                }

                if (direction.ToUpper() == "V")
                {     
                    int count = 0;
                    if (TryHorizontally(board, canBoatsTouch,row+length, col, length))
                    {
                        for (int j = 0; j < length; j++)
                        {
                            try
                            {
                                AddBoat(board, row + j, col);
                                if (canBoatsTouch == false)
                                {
                                    AddBoatSurrounding(board, row + j, col);

                                }
                                count++;
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                for (int k = 0; k < count; k++)
                                {
                                    DeleteBoat(row + k, col);
                                }
                                throw new ArgumentOutOfRangeException();
                                //code specifically for a ArgumentNullException
                            }
                        }
                    }
                    else
                    {
                        throw new ConstraintException();
                    }
                }
            }
        }
        
        private void AddBoat(List<List<BoardSquareState>> board, int row, int col)
        {        
            board[row][col].Ship = true;
            if (board == EnemyBoard)
            {
                board[row][col].isEnemyShip = true;
            }
        }
        
        private void AddBoatSurrounding(List<List<BoardSquareState>> board, int row, int col)
        {
            if (row == 0 && col == 0)
            {
                board[row+1][col].boatCannotTouch = true;
                board[row][col+1].boatCannotTouch = true;
            }
            else if (row == BoardRows && col == BoardCols)
            {
                board[row][col-1].boatCannotTouch = true;
                board[row-1][col].boatCannotTouch = true;
            }
            else if (row == 0 && col == BoardCols)
            {
                board[row][col-1].boatCannotTouch = true;
                board[row+1][col].boatCannotTouch = true;
            }
            else if (row == BoardRows && col == 0)
            {
                board[row-1][col].boatCannotTouch = true;
                board[row][col+1].boatCannotTouch = true;
            }
            else if (row == 0)
            {
                board[row][col-1].boatCannotTouch = true;
                board[row+1][col].boatCannotTouch = true;
                board[row][col+1].boatCannotTouch = true;
            }
            else if (row == BoardRows)
            {
                board[row][col-1].boatCannotTouch = true;
                board[row-1][col].boatCannotTouch = true;
                board[row][col+1].boatCannotTouch = true;
            }
            else if (col == 0)
            {
                board[row-1][col].boatCannotTouch = true;
                board[row+1][col].boatCannotTouch = true;
                board[row][col+1].boatCannotTouch = true;
            }
            else if (col == BoardCols)
            {
                board[row][col-1].boatCannotTouch = true;
                board[row-1][col].boatCannotTouch = true;
                board[row+1][col].boatCannotTouch = true;
            }
            else
            {
                board[row][col-1].boatCannotTouch = true;
                board[row-1][col].boatCannotTouch = true;
                board[row+1][col].boatCannotTouch = true;
                board[row][col+1].boatCannotTouch = true;
            }

        }
        
        private void DeleteBoat(int row, int col)
        {
            PlayerBoard[row][col].Ship = false;
        }
        
        public bool TryHorizontally(List<List<BoardSquareState>> board, bool canBoatsTouch, int x, int y, int length)
        {
            // Verify it fits
            if (canBoatsTouch)
            {
                for (int i = 0; i < length; ++i)
                {
                    if (board[x - i][y].Ship) 
                        return false;
                }
                return true;
            }
            else
            {
                for (int i = 0; i < length; ++i)
                {
                    if (board[x - i][y].Ship || board[x - i][y].boatCannotTouch) 
                        return false;
                }
                return true;
            }
        }
               
        private bool TryVertically(List<List<BoardSquareState>> board, bool canBoatsTouch, int x, int y, int length)
        {
            // Verify it fits
            if (canBoatsTouch)
            {
                for (int i = 0; i < length; ++i)
                {
                    if (board[x][y - i].Ship)
                        return false;
                }
                return true;
            }
            else
            {
                for (int i = 0; i < length; ++i)
                {
                    if (board[x][y - i].Ship || board[x][y - i].boatCannotTouch)
                        return false;
                }
                return true;
            }
        }
        
        private static void WaitForUser()
        {
            Console.Write("Press any key to go back to Menu!");
            Console.ReadKey();
        }
               
        public void UserAddBoat(int boatLength)
        {
            Console.WriteLine("Enter x and y coordinates to mark the starting point of the boat, followed with direction");
            GetUserInputAndPlaceBoat(boatLength);            
        }
        
        
        public void RandomizeBoatsOnBoard(List<List<BoardSquareState>> board, bool canBoatsTouch , List<int> boats)
        {
            _ships.Clear();
            foreach (var boat in boats)
            {
                if (boat == 1)
                {
                    _ships.Add(new Ship.Patrol());
                }
                if (boat == 2)
                {
                    _ships.Add(new Ship.Cruiser());
                }
                if (boat == 3)
                {
                    _ships.Add(new Ship.Submarine());
                }
                if (boat == 4)
                {
                    _ships.Add(new Ship.Battleship());
                }
                if (boat == 5)
                {
                    _ships.Add(new Ship.Carrier());
                }
            }                        
            for (int i = 0; i < boats.Count; i++)
            {
                int way = (int) (GetRandom.Next(0, 2));
                if (way == 0)
                {
                    int x = (int) (GetRandom.Next(0, BoardRows/2));
                    int y = (int) (x + _ships[i].Size);                    
                    if (TryVertically(board, canBoatsTouch, x, y, _ships[i].Size))
                    {
                        for (int j = 0; j < _ships[i].Size; j++)
                        {                  
                            AddBoat(board, x, y - j);
                            if (canBoatsTouch == false)
                            {
                                AddBoatSurrounding(board, x, y - j);

                            }
                        }
                    }
                    else
                    {
                        i--;
                    }                    
                }

                if (way == 1)
                {
                    int y = (int) (GetRandom.Next(0, BoardRows/2));
                    int x = (int) (y + _ships[i].Size);                   
                    if (TryHorizontally(board, canBoatsTouch, x, y, _ships[i].Size))
                    {
                        for (int j = 0; j < _ships[i].Size; j++)
                        {
                            AddBoat(board, x - j, y);
                            if (canBoatsTouch == false)
                            {
                                AddBoatSurrounding(board, x - j, y);

                            }
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
            }          
        }
        
        public String CheckBoardSquareState(List<List<BoardSquareState>> board, int row, int col)
        {
            if (board[row][col].Ship)
            {
                board[row][col].Visited = true;
                board[row][col].Hit = true;
                board[row][col].Ship = false;
                return "HIT!";
            }
            board[row][col].Visited = true;
            board[row][col].Miss = true;
            return "MISS!";
        }

        public void PlayGame()
        {
            Console.WriteLine("\nEnter x and y coordinates to shoot The Enemy!\n");
            
            do {
                
                bool isXgood = true;
                String getX;
                do
                {
                    Console.WriteLine("Enter x coordinate letter:");
                    String xCoordinate = Console.ReadLine()?.ToUpper();
                    getX = xCoordinate;
                    isXgood = ValidateUserXcoordinate(xCoordinate);
                    
                } while (isXgood);

                if (getX == "EXIT")
                {
                    break;
                }
                var x = YcoordinatesChar[getX];
                
                bool isYgood = true;
                String getY;
                do
                {
                    Console.WriteLine("Enter y coordinate letter:");
                    String yCoordinate = Console.ReadLine()?.ToUpper();
                    getY = yCoordinate;
                    isYgood = ValidateUserYcoordinate(yCoordinate);
                    
                } while (isYgood);
                
                if (getY == "EXIT")
                {
                    break;
                }
                          
                int y = int.Parse(getY);

                String result = CheckBoardSquareState(EnemyBoard, y - 1, x - 1);
                Console.WriteLine(GameUi.GetBoardString());
                Console.WriteLine("\n       YOU " + result);
                if (!EnemyBoard.Any(list => list.Any(s => s.Ship)))
                {
                    Console.WriteLine("YOU WIN!!!!");
                    WaitForUser();
                    break;
                }

                Console.WriteLine("\n" + "      ----- WAIT FOR OPPONENTS MOVE -----");
                System.Threading.Thread.Sleep(2000);
                int xRandom = (int) (GetRandom.Next(0, BoardRows));
                int yRandom = (int) (GetRandom.Next(0, BoardRows));
                String resultForEnemy = CheckBoardSquareState(PlayerBoard, xRandom, yRandom);
                Console.WriteLine(GameUi.GetBoardString());
                Console.WriteLine("\n       ENEMY "+resultForEnemy+"\n");
                if (!PlayerBoard.Any(list => list.Any(s => s.Ship)))
                {
                    Console.WriteLine("YOU LOST!!!");
                    WaitForUser();
                    break;
                }

            } while(EnemyBoard.Any(List => List.Any(s => s.Ship)) || 
                    PlayerBoard.Any(List => List.Any(s => s.Ship)));
        }
    }
}