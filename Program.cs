using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//Following two lines added to serialize game save and restore
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;

//Kaleb Gebrekirstos 
//Carrik McNerlin
//CS 305 Software Models
//Connect Four Game Mod.
//02/15/19
//The aim of this assignment was to develop a Connect Four game using an object-oriented 
//approach implemented in C#. Through the development of the program, 
//we got experience working with a classical PCM design and improved on our visual studio 
//.NET IDE skills.

//This is the class that contains the main method and calls on the initialize 
//function to start the game
namespace Program
{
    
    class Program : Game
    {
        const string saveFileName = "Connect4.xml";//path to saved serialized data
        [STAThread]
        static void Main(string[] args)
        {
            string input; //string object to check if user want to resume an old game 
            string fileLocation;//string object to reference the location of the file entered
            string inputFile; //string object to read in the text file to set board
            bool flag = false; //flag to break out of loop if the user wants to save the game
            bool flag2 = false; //flag to break out of game if input file is not found
            //Introducing the player to the game            
            Console.WriteLine("Welcome to our modified version of Connect Four.");
            Console.WriteLine("In our version, we don't deal with gravity.");
            Console.WriteLine("Thus, the player is free to choose wherever they want to place their colored disk.");
            Console.WriteLine("All the other rules of the game still apply.");

            try
            {
                Console.Write("Please enter file name: ");//Prompts user to enter file name for board size
                fileLocation = Console.ReadLine();
                StreamReader ifile = new StreamReader(fileLocation);
                inputFile = ifile.ReadLine();
                string[] digits = Regex.Split(inputFile, @"\D+"); //Get all digit sequence as strings
                sizeRow = int.Parse(digits[0]);
                sizeCol = int.Parse(digits[1]);
                ifile.Close();
            }

            catch(Exception e)
            {
                flag2 = true;
                Console.WriteLine(e.Message);
                System.Threading.Thread.Sleep(5000);
                if (flag2)
                {
                    return;
                }
            }        
            Game game = new Game();
            game.Initialize();//Initialize game 

            DrawBoard state = new DrawBoard();//Instance of DrawBoard class 
            int turn = 1;//Turn variable to keep track of how many moves played

            //added to retrieve saved game
            if (File.Exists(saveFileName))
            {
                Console.Write("Do you want to resume an old game? (Y/N)");
                input = Console.ReadLine();
                if (input[0] == 'y' || input[0] == 'Y')
                {
                    Stream saveFile = File.OpenRead(saveFileName);
                    SoapFormatter deserializer = new SoapFormatter();

                    //Added in a try catch block to handle file not found exception
                    try
                    {
                        game = (Game)(deserializer.Deserialize(saveFile));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    
                    saveFile.Close();
                }
                //remove file regardless of restore or not since a new game would be started.
                File.Delete(saveFileName);
            }
            //game not restored so start a new one
            if (game == null)
            {
                game = new Game();
                game.Initialize();
            }
                                    
            //main loop to prompt the user to enter col and row values as well as
            //to alternate turns and ask if they would like to save/restart the game
            while (!game.GameWon())
            {
                if (flag)
                    break;

                state.Display(game.board);
                                
                if (turn > 1)
                {
                    // Change active player (ignored if first turn of game)
                    if (game.currPlayer == 'B')
                    {
                        game.currPlayer = 'R';
                        Console.WriteLine(game.currPlayer);
                    }
                    else
                    {
                        game.currPlayer = 'B';
                        Console.WriteLine(game.currPlayer);
                    }
                }
                
                Console.WriteLine("Player " + game.currPlayer + "'s Turn");
                
                //Nested while loop to prevent overwriting of characters already in array
                //and to handle array out of bound exceptions
                bool validSpace = false;
                while (!validSpace)
                {
                    //Input column and row for placement and check if the user wants to
                    //save the game
                    Console.WriteLine("Do you wish to stop and save your game? \nIf yes, please " +
                        "enter 0, if not, please enter the desired row number (1-" + sizeRow + ")");
                    game.Row = Convert.ToInt32(Console.ReadLine());

                    //If user selects to end and save game, serialize game
                    if (game.Row == 0)
                    {
                        Stream saveFile = File.Create(saveFileName);
                        SoapFormatter serializer = new SoapFormatter();

                        try
                        {
                            serializer.Serialize(saveFile, game);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        saveFile.Close();
                    }
                    else
                    {
                        Console.WriteLine("Please enter the desired column number (1-"+sizeCol+")");
                        game.Col = Convert.ToInt32(Console.ReadLine());
                    }
                    
                    //If statement to break to update flag so that it can break straight
                    //to exit if the user chooses to save and exit 
                    if (game.Row == 0)
                    {
                        flag = true;
                        break;
                    }
                                        
                    //Displays message when the player goes out of bounds
                    if (game.Row < 1 || game.Row > sizeRow || game.Col < 1 
                        || game.Col > sizeCol)
                    {
                        Console.WriteLine("Sorry, you are out of bounds. Try again!");
                    }

                    else
                    {
                        //Insert character currPlayer into board[column-1][row-1] if space isn't already occupied
                        if (game.board[game.Row - 1, game.Col - 1] != ' ')
                        {
                            validSpace = false;
                            Console.WriteLine("Sorry, that space has already been selected.");
                            state.Display(game.board);
                        }
                        else
                        {
                            game.board[game.Row - 1, game.Col - 1] = game.currPlayer;
                            validSpace = true;
                        }
                    }
                    
                }
                // All spaces have been used without winner
                if (turn == sizeCol*sizeRow)
                {
                    Console.WriteLine("The game is a draw.");
                    break;
                }
                turn++;
            }
            
            if (game.GameWon())
            {
                Console.WriteLine("Congratuations player " + game.currPlayer + ", you are the winner!");
            }

            state.Display(game.board);
            System.Threading.Thread.Sleep(1000);

            EndState(); //End game
        }

        public static void EndState()
        {
            Console.WriteLine("Thank you for playing! Press the enter key to exit.");
            Console.Read();
        }
    }
}