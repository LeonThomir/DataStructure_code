using System;
namespace ChessGame_DataProject.Board
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Colors Color { get; set; }
        public int QuantityOfMoviments { get; set; }
        public BoardClass Board { get; set; }

        public Piece(Colors color, BoardClass board)
        {
            Position = null;
            Color = color;
            Board = board;
            QuantityOfMoviments = 0;
        }

        public void IncrementMovimentsQuantity()
        {
            QuantityOfMoviments++;
        }

        public void DecrementMovimentsQuantity()
        {
            QuantityOfMoviments--;
        }

        public bool ExistPossibleMoviments()//Trapped piece or not
        {
            bool[,] mat = PossibleMoviments();
            for (int i = 0; i < Board.Lines; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanMoveTo(Position position)
        {
            return PossibleMoviments()[position.Line, position.Column];
        }

        public abstract bool[,] PossibleMoviments(); //Abstract methods to be implemented in each class piece
    }
}
