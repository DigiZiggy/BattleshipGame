using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Ship
    {
        public int ShipId { get; set; }
               
        [MaxLength(34)] 
        public string Name { get; set; }
        public int Size { get; set; }        
        
        public class Carrier: Ship
        {
            public Carrier()
            {
                Name = "Carrier";
                Size = 5;

            }
        }
        
        public class Battleship: Ship
        {
            public Battleship()
            {
                Name = "Battleship";
                Size = 4;
            }
        }
        
        public class Submarine: Ship
        {
            public Submarine()
            {
                Name = "Submarine";
                Size = 3;
            }
        }

        public class Cruiser: Ship
        {
            public Cruiser()
            {
                Name = "Cruiser";
                Size = 2;
            }
        }

        public class Patrol: Ship
        {
            public Patrol()
            {
                Name = "Patrol";
                Size = 1;
            }
        }
    }
}