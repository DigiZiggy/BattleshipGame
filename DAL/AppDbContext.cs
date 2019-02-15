using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameState> GameStates { get; set; }
        
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information, true)
            });

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseMySQL(
                    "server=alpha.akaver.com;" + // server to use
                    "database=student2018_179595IADBSigrid;" + // database to use or create
                    "user=student2018;" + // no credentials needed, this is local sql instance
                    "password=student2018" // allow multiple  parallel queries
                );
        }
    }
}