using System;
using System.Data.SqlClient;
using System.Linq;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Initializers
{
    public class Initialise
    {
        public static AppDbContext DbContext = new AppDbContext();

        public static void SerializeGameAndSave(Game game)
        {

            DbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE GameStates");
            
            string output = JsonConvert.SerializeObject(game);
    
            var savedGame = new GameState()
            {
                gameState = output
            };

            DbContext.Add(savedGame);
            DbContext.SaveChanges();
    
        }
        
        public static Game LoadAndDeserializeGame()
        {
 
            var gameState = DbContext.GameStates
                .Single(b => b.GameStateId == 1);
            
            Game deserializedGame = JsonConvert.DeserializeObject<Game>(gameState?.gameState);

            return deserializedGame;

        }
        
    }
}