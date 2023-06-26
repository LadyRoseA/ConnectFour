using System;
using System.Numerics;

namespace Connect4
{
    //Game Component Abstract Class
    public abstract class GameComponent
    {
        public abstract void Play();
    }

    //Player Info Class
    public class PlayerInfo
    {
        public string PlayerName { get; set; }
        public char PlayerSymbol { get; set; }
    }

    //Board Class
    class Board
    {
        public static void GameBoard(char[,] board)
        {
            int rows = 6;
            int columns = 7;
            int i, ix;
            Console.WriteLine("------- Connect 4 Game -------");
            Console.WriteLine();
            Console.WriteLine("   1   2   3   4   5   6   7");
            for (i = 1; i <= rows; i++)
            {
                //Console.Write(" |  ");
                for (ix = 1; ix <= columns; ix++)
                {
                    if (board[i, ix] != 'X' && board[i, ix] != 'O')
                        board[i, ix] = ' ';

                    //Console.Write(board[i, ix]);
                    Console.Write($" | {board[i, ix]}");

                }
                Console.WriteLine(" | ");
                //Console.Write(" | \n");
            }
            Console.WriteLine("_______________________________");
        }

        public static int FilledBoard(char[,] board)
        {
            int full = 0;
            for (int i = 1; i <= 7; ++i)
            {
                if (board[1, i] != ' ')
                    ++full;
            }

            return full;
        }
    }

    //GamePlay Class inherited from GameComponent
    public class GamePlay : GameComponent
    {
        public override void Play()
        {
        }

        public static int PlayerDrop(char[,] board, PlayerInfo activePlayer)
        {
            int dropChoice;
            Console.WriteLine();
            Console.WriteLine(activePlayer.PlayerName + "'s Turn ");
            do
            {
                Console.Write("Choose a number from 1 to 7: ");
                dropChoice = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            } while (dropChoice < 1 || dropChoice > 7);

            while (board[1, dropChoice] == 'X' || board[1, dropChoice] == 'O')
            {
                Console.Clear();
                Board.GameBoard(board);//Reprint board after error handling
                Console.WriteLine();
                Console.Write("That column is full, select another number from 1 to 7: ");
                dropChoice = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
            }
            return dropChoice;
        }


        public static int CheckFour(char[,] board, PlayerInfo activePlayer)
        {
            char XO;
            int win;

            XO = activePlayer.PlayerSymbol;
            win = 0;

            for (int i = 6; i >= 1; --i)
            {
                for (int ix = 7; ix >= 1; --ix)
                {
                    if (board[i, ix] == XO &&
                        board[i - 1, ix - 1] == XO &&
                        board[i - 2, ix - 2] == XO &&
                        board[i - 3, ix - 3] == XO)
                    {
                        win = 1;
                    }

                    if (board[i, ix] == XO &&
                        board[i, ix - 1] == XO &&
                        board[i, ix - 2] == XO &&
                        board[i, ix - 3] == XO)
                    {
                        win = 1;
                    }

                    if (board[i, ix] == XO &&
                        board[i - 1, ix] == XO &&
                        board[i - 2, ix] == XO &&
                        board[i - 3, ix] == XO)
                    {
                        win = 1;
                    }

                    if (board[i, ix] == XO &&
                        board[i - 1, ix + 1] == XO &&
                        board[i - 2, ix + 2] == XO &&
                        board[i - 3, ix + 3] == XO)
                    {
                        win = 1;
                    }

                    if (board[i, ix] == XO &&
                         board[i, ix + 1] == XO &&
                         board[i, ix + 2] == XO &&
                         board[i, ix + 3] == XO)
                    {
                        win = 1;
                    }
                }
            }

            return win;
        }

        public static void PlayerWin(PlayerInfo activePlayer)
        {
            Console.WriteLine();
            Console.WriteLine(activePlayer.PlayerName + ", CONGRATULATIONS! YOU WIN!");
        }

        public static void CheckBellow(char[,] board, PlayerInfo activePlayer, int dropChoice)
        {
            int length = 6;
            int turn = 0;

            do
            {
                if (board[length, dropChoice] != 'X' && board[length, dropChoice] != 'O')
                {
                    board[length, dropChoice] = activePlayer.PlayerSymbol;
                    turn = 1;
                }
                else
                    --length;
            } while (turn != 1);
        }
    }

    //Game Restart inherited from GameComponent
    public class Restart : GameComponent
    {
        public override void Play()
        {
        }

        public static int PlayAgain(char[,] board)
        {
            int restart;

            Console.Write("Would you like to play again? Yes(1) No(2): ");
            restart = int.Parse(Console.ReadLine());
            if (restart == 1)
            {
                Console.Clear();//Clear screen
                for (int i = 1; i <= 6; i++)
                {
                    for (int ix = 1; ix <= 7; ix++)
                    {
                        board[i, ix] = ' ';
                    }
                }
                Board.GameBoard(board);//Set new board
            }
            else
                Console.WriteLine("Thank you for playing. See you again!");
            return restart;
        }
    }

    //GameManager Class
    public class GameManager
    {
        public void RunGame(PlayerInfo playerOne, PlayerInfo playerTwo)
        {
            //PlayerInfo playerOne = new PlayerInfo();
            //PlayerInfo playerTwo = new PlayerInfo();

            char[,] board = new char[9, 10];
            int dropChoice, win, full, again;

            /*Console.WriteLine("Let's play Connect 4. Let's go!");
            Console.WriteLine();
            Console.Write("Please enter PLAYER 1 name: ");
            playerOne.PlayerName = Console.ReadLine();
            playerOne.PlayerSymbol = 'X';
            Console.Write("Please enter PLAYER 2 name: ");
            playerTwo.PlayerName = Console.ReadLine();
            playerTwo.PlayerSymbol = 'O';*/

            Console.Clear();//Clear screen and start game
            again = 0;
            Board.GameBoard(board);//Initialize game board print
            do
            {
                dropChoice = GamePlay.PlayerDrop(board, playerOne);
                GamePlay.CheckBellow(board, playerOne, dropChoice);
                Board.GameBoard(board);
                win = GamePlay.CheckFour(board, playerOne);
                if (win == 1)
                {
                    GamePlay.PlayerWin(playerOne);
                    again = Restart.PlayAgain(board);

                    if (again == 2)
                    {
                        break;
                    }
                }

                dropChoice = GamePlay.PlayerDrop(board, playerTwo);
                GamePlay.CheckBellow(board, playerTwo, dropChoice);
                Board.GameBoard(board);
                win = GamePlay.CheckFour(board, playerTwo);
                if (win == 1)
                {
                    GamePlay.PlayerWin(playerTwo);
                    again = Restart.PlayAgain(board);
                    if (again == 2)
                    {
                        break;
                    }
                }
                full = Board.FilledBoard(board);
                if (full == 7)
                {
                    Console.WriteLine("OOPS! IT'S A DRAW GAME!");
                    again = Restart.PlayAgain(board);
                }

            } while (again != 2);
        }
    }

    //Game Option Class
    class GameOption
    {
        public void Display()
        {
            Console.WriteLine("------- Connect 4 Game -------");
            Console.WriteLine();
            Console.WriteLine("Please select game mode:");
            Console.WriteLine();
            Console.WriteLine("(1) Two Player Human");
            Console.WriteLine("(2) Two Player Human vs AI");
            Console.WriteLine("(3) Exit Game");
            Console.WriteLine();
            Console.Write("Select your choice (1-3): ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.Clear();
                    PlayerInfo playerOne = new PlayerInfo();
                    PlayerInfo playerTwo = new PlayerInfo();

                    Console.WriteLine("Let's play Connect 4. Let's go!");
                    Console.WriteLine();
                    Console.WriteLine("Two Player Human Game Mode");
                    Console.WriteLine("--------------------------");
                    Console.Write("Please enter PLAYER 1 name: ");
                    playerOne.PlayerName = Console.ReadLine();
                    playerOne.PlayerSymbol = 'X';
                    Console.Write("Please enter PLAYER 2 name: ");
                    playerTwo.PlayerName = Console.ReadLine();
                    playerTwo.PlayerSymbol = 'O';
                    GameManager gameManager = new GameManager();
                    gameManager.RunGame(playerOne, playerTwo); // Pass player information to RunGame method
                    break;
                case "2":
                    // ...
                    break;
                case "3":
                    Console.WriteLine();
                    Console.WriteLine("Thanks for playing!");
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid input. Please select a valid option.");
                    Console.WriteLine();
                    Display();
                    break;
            }
        }
    }

    //Main Program Class
    public class Program
    {
        public static void Main()
        {
            GameOption option = new GameOption();
            option.Display();
        }
    }

}
