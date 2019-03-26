using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This is the class that checks to see if a win/lose condition occurs is inherited by both other classes 
//Declaration of protected and public variables occurs here too
namespace Program
{
    [Serializable]
    class Game
    { 
        private int col;
        private int row;
        public int Row { get; set; }//Setter and getter for row  
        public int Col { get; set; }//Setter and getter for col
        public static int sizeRow;
        public static int sizeCol;
        public char currPlayer;//Character to keep track of current player        
        public char[,] board = new char[sizeRow, sizeCol];//board matrix

        //Default constructor
        public Game()
        {
            row = 0;
            col = 0;
            currPlayer = ' ';
        }
        //Constructor
        public Game(int r, int c, char p)
        {
            row = r;
            col = c;
            currPlayer = p;
        }
        //Function to start the game and prompt user to begin the game
        public void Initialize()
        {
            //Input what color the player wants to be
            Console.WriteLine("To begin playing, please first select a color. Black or Red (B/R)?");
            currPlayer = Console.ReadKey().KeyChar;
            Console.WriteLine();
            Console.WriteLine("Welcome player 1. You have selected {0}. Let us begin.", currPlayer);
            Console.WriteLine();

            //Initialize the board 
            for (int i = 0; i < sizeRow; i++)
            {
                for (int j = 0; j < sizeCol; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }
        //Check method to see if there are 4 disks of the same color connected 
        public bool GameWon()
        {
            for (int row = 0; row < sizeRow; row++)
            {
                for (int col = 0; col < sizeCol; col++)
                {
                    //Used to make sure that the center position is the selected one
                    string verticalCheck = "";
                    string horizontalCheck = "";
                    string downLeftCheck = "";
                    string downRightCheck = "";

                    try
                    {
                        for (int i = -3; i < 4; i++)//grid across: -3, -2 -1, 0 (designated center, slightly off), 1, 2, 3
                        {
                            //Checks to see if four are connected horizontally/vertically/diagonally
                            if (row + i >= 0 && row + i < sizeRow)
                            {
                                verticalCheck += board[row + i, col];

                                if (verticalCheck.IndexOf("BBBB") != -1 || verticalCheck.IndexOf("RRRR") != -1)
                                    return true;
                            }

                            if (col + i >= 0 && col + i < sizeCol)
                            {
                                horizontalCheck += board[row, col + i];

                                if (horizontalCheck.IndexOf("BBBB") != -1 || horizontalCheck.IndexOf("RRRR") != -1)
                                    return true;
                            }

                            if (row + i >= 0 && row + i < sizeRow && col - i >= 0 && col - i < sizeCol)
                            {
                                downLeftCheck += board[row + i, col - i];

                                if (downLeftCheck.IndexOf("BBBB") != -1 || downLeftCheck.IndexOf("RRRR") != -1)
                                    return true;
                            }

                            if (row + i >= 0 && row + i < sizeRow && col + i >= 0 && col + i < sizeCol)
                            {
                                downRightCheck += board[row + i, col + i];

                                if (downRightCheck.IndexOf("BBBB") != -1 || downRightCheck.IndexOf("RRRR") != -1)
                                    return true;
                            }
                        } // End i
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The board size has changed after the game was saved. " +
                            "You must now start a new game.");
                        System.Threading.Thread.Sleep(5000);
                        System.Environment.Exit(1);
                    }
                } // End col

            } // End row

            // All win options attempted, no win found
            return false;
        }

    }
}