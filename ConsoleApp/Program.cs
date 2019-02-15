using System;
using DAL;
using MenuSystem;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Battleship!");
            new ApplicationMenu().RunMenu();        
        }
    }
}