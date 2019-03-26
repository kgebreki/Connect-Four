using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Class to display current state of the board
namespace Program
{

    /* GRID ORDER (row,column)
     * [0,0] [0,1] [0,2] [0,3] ... [0,7]
     * [1,0] [1,1] [1,2] [1,3] ... [1,7]
     * [2,0] [2,1] [2,2] [2,3] ... [2,7]
     * ...
     */

    class DrawBoard : Game
    {
        //Display board configuration
        public void Display(char[,] board)
        {
            int index = 1;
            Console.Write("   ");
            for (int i = 0; i < sizeCol; i++)
            {
                Console.Write(index+"    ");
                index++;
            }
            Console.WriteLine();
            for (int i = 0; i < sizeRow; i++)
            {
                Console.Write(i + 1 + " ");
                for (int j = 0; j < sizeCol; j++)
                {
                    Console.Write("[" + board[i, j] + "]  ");
                }
                Console.WriteLine();
            }
        }
    }
}
