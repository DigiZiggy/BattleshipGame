using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Board
    {
        public int BoardId { get; set; }

        private readonly int _boardRows = Game.BoardSize;
        private readonly int _boardCols = Game.BoardSize;
        
        public List<List<BoardSquareState>> PlayerBoard { get; set; } = new List<List<BoardSquareState>>();        
        public List<List<BoardSquareState>> EnemyBoard { get; set; } = new List<List<BoardSquareState>>();

        public Board()
        {
        }
        public Board(Player player)
        {
            for (int i = 0; i < _boardRows; i++)
            {
                List<BoardSquareState> playerSquaresList = new List<BoardSquareState>();
                List<BoardSquareState> enemySquaresList = new List<BoardSquareState>();
                for (int j = 0; j < _boardCols; j++)
                {
                    BoardSquareState playerSquare = new BoardSquareState();
                    BoardSquareState enemySquare = new BoardSquareState();
                    playerSquaresList.Add(playerSquare);
                    enemySquaresList.Add(enemySquare);                

                }
                PlayerBoard.Add(playerSquaresList);
                EnemyBoard.Add(enemySquaresList);
            }
        }
    }
}