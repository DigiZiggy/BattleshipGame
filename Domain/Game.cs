using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Game
    {
        public int GameId { get; set; }
        public static int BoardSize { get; set; } = 10;        
        public static bool CanShipsTouch { get; set; } = true;       
        public static List<int> ShipsCountAndSizes { get; set; } = new List<int>
        {
            5, 4, 3, 2, 1
        };
        
        //Foreign key for Player
        public int PlayerId { get; set; }
        public Player Player { get; set; }
                
        public int BoardId { get; set; }
        public Board Board { get; set; }

        public Game()
        {
            
        }
        
        public Game(int boardSize, bool shipTouch, List<int> ships, Player player, Board board)
        {
            BoardSize = boardSize;
            CanShipsTouch = shipTouch;
            ShipsCountAndSizes = ships;
            Player = player;
            Board = board;

        }
 
        public static void ShipSizeToString()
        {
            foreach (var size in ShipsCountAndSizes)
            {
                Console.WriteLine(size);
            }
        }
    }
}