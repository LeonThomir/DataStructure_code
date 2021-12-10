using System;
using ChessGame_DataProject.Board;
using ChessGame_DataProject.Chess;
using ChessGame_DataProject.Exceptions;

namespace ChessGame_DataProject
{
    public class Driver
    {
        public static void NewChessGame()
        {
            try
            {
                ChessMatch match = new ChessMatch();

                while (!match.Finish)
                {
                    try
                    {
                        Console.Clear();
                        Screen.PrintMatch(match);
                        Console.WriteLine();
                        Screen.PrintCapturedPieces(match);
                        Console.WriteLine();
                        ChessMatch.KnowAdvantage(match);
                        Console.WriteLine($"The evaluated position is {match.Advantage}");
                        Console.WriteLine();

                        Console.Write("Origin: ");
                        Position origin = Screen.ReadChessPosition().ToPosition();
                        match.ValidOriginPosition(origin);

                        bool[,] possiblePositions = match.Board.Piece(origin).PossibleMoviments();

                        Console.Clear();
                        Screen.PrintBoard(match.Board, possiblePositions);

                        Console.WriteLine("\n");

                        Console.Write("Destiny: ");
                        Position destiny = Screen.ReadChessPosition().ToPosition();
                        match.ValidDestinyPosition(origin, destiny);

                        match.RealizeMove(origin, destiny);
                    }
                    catch (BoardException e)
                    {
                        Screen.PrintException(e);
                    }
                }
                Console.Clear();
                Screen.PrintMatch(match);
            }
            catch (BoardException e)
            {

                Screen.PrintException(e);
            }
        }
        public static void NewEconomyChessGame()
        {
            try
            {
                ChessMatch match = new ChessMatch();

                while (!match.Finish)
                {
                    try
                    {
                        Screen.PrintMatch(match);
                        Console.WriteLine($"White Golds : {match.Golds[0]} Gains : {match.Gains[0]}");
                        Console.WriteLine($"Black Golds: {match.Golds[1]} Gains : {match.Gains[1]}");

                        Console.WriteLine();
                        Console.WriteLine("What do you want to do?");
                        Console.WriteLine("Move a piece : Type 1");
                        Console.WriteLine("Earn gold : Type 2");
                        Console.WriteLine("Buy a new building : Type 3");
                        Console.WriteLine("Make two moves in a row : Type 4");
                        Console.WriteLine("Buy an atomic missile : Type 5");
                        Console.WriteLine("If you are in check, you must move a piece.");
                        Console.WriteLine();
                        string answer = Console.ReadLine();
                        if (answer == "1")
                        {
                            Console.Write("Origin: ");
                            Position origin = Screen.ReadChessPosition().ToPosition();
                            match.ValidOriginPosition(origin);

                            bool[,] possiblePositions = match.Board.Piece(origin).PossibleMoviments();

                            Console.Clear();
                            Screen.PrintBoard(match.Board, possiblePositions);

                            Console.WriteLine("\n");

                            Console.Write("Destiny: ");
                            Position destiny = Screen.ReadChessPosition().ToPosition();
                            match.ValidDestinyPosition(origin, destiny);

                            match.RealizeMove(origin, destiny);
                        }
                        else if (answer == "2")
                        {
                            Console.Clear();
                            ChessMatch.EarnGold(match);
                            match.ChangePlayer();
                            match.Turn++;
                        }
                        else if (answer == "3")
                        {
                            Console.WriteLine();
                            ChessMatch.BuyBuilding(match);
                            match.ChangePlayer();
                            match.Turn++;
                        }
                        else if (answer=="4")
                        {
                            Console.Clear();
                            ChessMatch.TwoMoves(match);
                            match.ChangePlayer();
                            match.Turn++;
                        }
                        else if (answer=="5")
                        {
                            Console.Clear();
                            ChessMatch.AtomicMissile(match);
                            match.ChangePlayer();
                            match.Turn++;
                        }
                    }
                    catch (BoardException e)
                    {
                        Screen.PrintException(e);
                    }
                }
                Console.Clear();
                Screen.PrintMatch(match);
            }
            catch (BoardException e)
            {

                Screen.PrintException(e);
            }
        }
    }
}
