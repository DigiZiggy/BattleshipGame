using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; } 

        [Required]
        [MaxLength(34)]
        public string Name { get; set; }
        
        public Player()
        {
        }
        public Player(String name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"ID: {PlayerId}; Name: {Name}";
        }
    }
}
