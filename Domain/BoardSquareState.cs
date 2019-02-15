using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class BoardSquareState
    {
        public int BoardSquareStateId { get; set; }
        public bool Ship { get; set; } = false;
        public bool Hit { get; set; } = false;
        public bool Miss { get; set; } = false;
        public bool Visited { get; set; } = false;         
        public bool isEnemyShip { get; set; } = false;        
        public bool boatCannotTouch { get; set; } = false;
        
        public BoardSquareState()
        {
            this.Visited = Visited;
            this.Hit = Hit;
            this.Miss = Miss;
            this.isEnemyShip = isEnemyShip;

        }
        
        public override string ToString()
        {
            if (Visited)
            {
                return Hit ? "X" : "-";
            }
            if (Ship)
            {
                return isEnemyShip ? " " : "#";
            }
            return " ";
        }
    }
}