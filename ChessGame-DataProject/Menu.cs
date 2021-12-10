using System;
namespace ChessGame_DataProject
{
    public class Menu
    {
        public static void GameMenu() //Display a basic Game menu
        {
            Console.WriteLine("What game do you want to play?");
            Console.WriteLine("Normal chess : Type 1");
            Console.WriteLine("Economic chess game : Type 2");
            string answer = Console.ReadLine();
            if (answer=="1")
            {
                Driver.NewChessGame();
            }
            else if (answer=="2")
            {
                Driver.NewEconomyChessGame();
            }
            else
            {
                Console.WriteLine("There is an error. Please try again.");
                Console.WriteLine();
                GameMenu();
            }
        }
    }
}
