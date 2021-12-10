using System;
using ChessGame_DataProject.Exceptions;

namespace ChessGame_DataProject.Board
{
    class BoardClass
    {
        public int Lines { get; set; }
        public int Columns { get; set; }
        public Piece[,] Pieces;

        public BoardClass(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
            Pieces = new Piece[lines, columns];
        }

        public Piece Piece(int line, int column)
        {
            return Pieces[line, column];
        }

        public Piece Piece(Position position)
        {
            return Pieces[position.Line, position.Column];
        }

        public void PutPiece(Piece piece, Position position)
        {
            if (ExistPieceInPosition(position))
            {
                throw new BoardException("Exist a piece in this position!!");
            }
            Pieces[position.Line, position.Column] = piece;
            piece.Position = position;
        }

        public Piece RemovePiece(Position position)
        {
            if (Piece(position) == null)
            {
                return null;
            }

            Piece auxiliar = Piece(position);
            auxiliar.Position = null; //We reset position

            Pieces[position.Line, position.Column] = null; //We also have to remove the piece from the 2 dimensionnal array Pieces
            return auxiliar;
        }

        public bool ExistPieceInPosition(Position position)
        {
            ValidPosition(position); //check if it's in the board
            return Piece(position) != null; //check if there is a piece
        }

        public bool VerifyPosition(Position position)
        {
            if (position.Line >= Lines || position.Line < 0 || position.Column >= Columns || position.Column < 0) //Check if in the board size
            {
                return false;
            }
            return true;
        }

        public void ValidPosition(Position position) //Exception for out of the board
        {
            if (!VerifyPosition(position))
            {
                throw new BoardException("Invalid position!!");
            }
        }
    }
}
