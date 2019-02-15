using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Initializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        public Game loadedGame;
        public Board board;
        public Player player;
        public static int boardSize;
        public bool canBoatsTouch;
        public List<int> ShipsList;


        public void OnGet()
        {
        }
        
        public void OnPostLoadGame()
        {
            loadedGame = Initialise.LoadAndDeserializeGame();
            player = loadedGame.Player;
            board = loadedGame.Board;
            boardSize = Game.BoardSize;
            canBoatsTouch = Game.CanShipsTouch;
            ShipsList = Game.ShipsCountAndSizes;
            
        } 
    }
}