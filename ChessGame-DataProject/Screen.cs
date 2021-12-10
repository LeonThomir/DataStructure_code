using System;
using System.Collections.Generic;
using ChessGame_DataProject.Board;
using ChessGame_DataProject.Chess;
using ChessGame_DataProject.Exceptions;

namespace ChessGame_DataProject
{

    class Screen
    {
        public static void PrintMatch(ChessMatch match) //General informations about the current turn
        {
            Console.WriteLine();
            PrintBoard(match.Board);

            Console.WriteLine();

            Console.WriteLine($"Turn: {match.Turn}");

            if (!match.Finish)
            {
                Console.WriteLine($"Waiting move: {match.CurrentPlayer}");

                if (match.Check)
                {
                    Console.WriteLine("Check!");
                }
            }
            else
            {
                Console.WriteLine("Checkmate!");
                Console.WriteLine($"Winner: {match.CurrentPlayer}");
            }

        }

        public static void PrintCapturedPieces(ChessMatch match) //Allows players to see which pieces are captured
        {
            Console.WriteLine("Captured pieces:");

            Console.Write($"White: ");
            PrintSet(match.CapturedPiece(Colors.White));

            Console.WriteLine();

            ConsoleColor auxiliar = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow; //We change color of the background so that we can see all pieces if the console is in black or white mode

            Console.Write($"Black: ");
            PrintSet(match.CapturedPiece(Colors.Black));
            Console.ForegroundColor = auxiliar; //We give back the orginal setting color
            Console.WriteLine();

        }

        public static void PrintSet(HashSet<Piece> pieces) //Allows us to print captured pieces
        {
            Console.Write("[");
            foreach (Piece piece in pieces)
            {
                Console.Write($"{piece} ");
            }
            Console.Write("]");
        }

        public static void PrintBoard(BoardClass board) //Print the original board
        {
            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    PrintPiece(board.Piece(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void PrintBoard(BoardClass board, bool[,] possiblePositions) //Print the board with possible possitions for the move higlighted in Dark blue
        {
            ConsoleColor originalBackground = Console.BackgroundColor; //Same idea than for the captured pieces
            ConsoleColor alteredBackground = ConsoleColor.DarkBlue;

            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    if (possiblePositions[i, j])
                    {
                        Console.BackgroundColor = alteredBackground;
                    }
                    else
                    {
                        Console.BackgroundColor = originalBackground;
                    }
                    PrintPiece(board.Piece(i, j));
                    Console.BackgroundColor = originalBackground;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");

            Console.BackgroundColor = originalBackground; //Same idea than for the captured pieces
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null)
            {
                Console.Write("= ");
            }
            else
            {
                if (piece.Color == Colors.White)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(piece);
                    Console.ForegroundColor = color; //Same idea than for the captured pieces
                }
                Console.Write(" ");
            }
        }

        public static ChessPosition ReadChessPosition() //Allows to change string console inputs to a ChessPositions directly
        {
            string auxiliar = Console.ReadLine();
            char column = auxiliar[0];
            int line = int.Parse(auxiliar[1] + "");
            return new ChessPosition(column, line);
        }

        public static void PrintException(BoardException e) //BoardException printer
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Press ENTER to continue!");
            Console.ReadLine();
        }
    }
}
