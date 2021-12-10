using System;
using ChessGame_DataProject.Board;
using ChessGame_DataProject.Chess;

namespace ChessGame_DataProject.Pieces
{
    class King : Piece
    {

        private ChessMatch match;


        public King(Colors color, BoardClass board, ChessMatch match) : base(color, board)
        {
            this.match = match;
        }

        public override string ToString()
        {
            return "K";
        }

        private bool TestRookToRoque(Position position) //To do small and big roque
        {
            Piece piece = Board.Piece(position);
            return piece != null && piece is Rook && piece.Color == Color && piece.QuantityOfMoviments == 0;
        }

        private bool ThisMovimentIsPossible(Position position)
        {
            Piece piece = Board.Piece(position);
            return piece == null || piece.Color != Color;
        }

        public override bool[,] PossibleMoviments() //Implements the method with king moves
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];

            Position pos = new Position(0, 0);

            // North
            pos.DefineValues(Position.Line - 1, Position.Column);

            if (Board.VerifyPosition(pos) && ThisMovimentIsPossible(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Northeast
            pos.DefineValues(Position.Line - 1, Position.Column + 1);

            if (Board.VerifyPosition(pos) && ThisMovimentIsPossible(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // East
            pos.DefineValues(Position.Line, Position.Column + 1);

            if (Board.VerifyPosition(pos) && ThisMovimentIsPossible(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Southeast
            pos.DefineValues(Position.Line + 1, Position.Column + 1);

            if (Board.VerifyPosition(pos) && ThisMovimentIsPossible(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // South
            pos.DefineValues(Position.Line + 1, Position.Column);

            if (Board.VerifyPosition(pos) && ThisMovimentIsPossible(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Southwest
            pos.DefineValues(Position.Line + 1, Position.Column - 1);

            if (Board.VerifyPosition(pos) && ThisMovimentIsPossible(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // West
            pos.DefineValues(Position.Line, Position.Column - 1);

            if (Board.VerifyPosition(pos) && ThisMovimentIsPossible(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Northwest
            pos.DefineValues(Position.Line - 1, Position.Column - 1);

            if (Board.VerifyPosition(pos) && ThisMovimentIsPossible(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Roque - Especial move
            if (QuantityOfMoviments == 0 && !match.Check)
            {
                // Small roque
                Position positionRook = new Position(Position.Line, Position.Column + 3);
                if (TestRookToRoque(positionRook))
                {
                    Position auxiliar1 = new Position(Position.Line, Position.Column + 1);
                    Position auxiliar2 = new Position(Position.Line, Position.Column + 2);

                    if (Board.Piece(auxiliar1) == null && Board.Piece(auxiliar2) == null)
                    {
                        mat[Position.Line, Position.Column + 2] = true;
                    }
                }

                // Big roque
                Position positionRook2 = new Position(Position.Line, Position.Column - 4);
                if (TestRookToRoque(positionRook2))
                {
                    Position auxiliar1 = new Position(Position.Line, Position.Column - 1);
                    Position auxiliar2 = new Position(Position.Line, Position.Column - 2);
                    Position auxiliar3 = new Position(Position.Line, Position.Column - 3);

                    if (Board.Piece(auxiliar1) == null && Board.Piece(auxiliar2) == null && Board.Piece(auxiliar3) == null)
                    {
                        mat[Position.Line, Position.Column - 2] = true;
                    }
                }
            }
            return mat;
        }
    }
}
