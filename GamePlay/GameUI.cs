using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace GamePlay
{
    public class GameUI
    {
        private readonly int BoardRows = Game.BoardSize;
        public int BoardCols = Game.BoardSize;
        private Player Player;
        private readonly List<List<BoardSquareState>> EnemyBoard;
        private readonly List<List<BoardSquareState>> PlayerBoard;

        public GameUI(Player player, Board board)
        {
            Player = player;
            EnemyBoard = board.EnemyBoard;
            PlayerBoard = board.PlayerBoard;
        }
        
        public string GetBoardString()
        {
            var sb = new StringBuilder();
            
            List<string> colSymbol = new List<string>
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V"
            };

            sb.Append("    ");
            sb.Append("------------------- YOUR BOARD ------------------");
            sb.Append("   ");
            sb.Append("\n"+"\n");
            sb.Append("   ");

            for (int k = 0; k < BoardRows; k++)
            {
                sb.Append("  " + colSymbol[k] + "  ");

            }
            sb.Append("\n");

            int i = 1;
            int j = 1;

            foreach (var boardRow in PlayerBoard)
            {

                if (i == BoardRows)
                {
                    sb.Append("   ");
                    sb.Append(GetRowSeparator(boardRow.Count) +"\n");
                    if (i < 10)
                    {
                        sb.Append(i + "  ");

                    }
                    else
                    {
                        sb.Append(i + " ");

                    }
                    sb.Append(GetRowWithData(boardRow) + "\n");
                    break;
                }
                if (i >= 10)
                {
                    sb.Append("   ");
                    sb.Append(GetRowSeparator(boardRow.Count) +"\n");
                    sb.Append(i + " ");
                    sb.Append(GetRowWithData(boardRow) + "\n");
                    i++;
                }
                if (i < 10)
                {
                    sb.Append("   ");
                    sb.Append(GetRowSeparator(boardRow.Count) +"\n");
                    sb.Append(i + "  ");
                    sb.Append(GetRowWithData(boardRow) + "\n");
                    i++;
                }           
            }
            
            sb.Append("   ");
            sb.Append(GetRowSeparator(BoardRows));
           
            sb.Append("\n"+"\n"+"   ------------------- ENEMY BOARD ------------------\n"+"\n");
            sb.Append("   ");

            for (int k = 0; k < BoardRows; k++)
            {
                sb.Append("  " + colSymbol[k] + "  ");

            }
            sb.Append("\n");


            
            foreach (var row in EnemyBoard)
            {
                if (j == BoardRows)
                {
                    sb.Append("   ");
                    sb.Append(GetRowSeparator(row.Count) + "\n");
                    if (j < 10)
                    {
                        sb.Append(j + "  ");

                    }
                    else
                    {
                        sb.Append(j + " ");

                    }                    
                    sb.Append(GetRowWithData(row) + "\n");
                    break;
                }
                
                if (j >= 10)
                {
                    sb.Append("   ");
                    sb.Append(GetRowSeparator(row.Count) +"\n");
                    sb.Append(j + " ");
                    sb.Append(GetRowWithData(row) + "\n");
                    j++;
                }
                if (j < 10)
                {
                    sb.Append("   ");
                    sb.Append(GetRowSeparator(row.Count) +"\n");
                    sb.Append(j + "  ");
                    sb.Append(GetRowWithData(row) + "\n");
                    j++;
                }
            }            
            sb.Append("   ");
            sb.Append(GetRowSeparator(BoardRows));
            
            return sb.ToString();
        }
        
        public string GetRowSeparator(int elemCountInRow)
        {
            var sb = new StringBuilder();
     
            for (int i = 0; i < elemCountInRow; i++)
            {
                sb.Append("+----");
            }
                      
            sb.Append("+");
            return sb.ToString();
        }

        public string GetRowWithData(List<BoardSquareState> boardRow)
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var boardSquareState in boardRow)
            {
                sb.Append("|  " + Convert.ToString(boardSquareState) + " ");
                i++;
            }

            sb.Append("|");
            return sb.ToString();
        }       
    }
}