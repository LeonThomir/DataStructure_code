using System;
using ChessGame_DataProject.Board;
using ChessGame_DataProject.Chess;

namespace ChessGame_DataProject.Pieces
{
    class Pawn : Piece
    {
        private ChessMatch match;


        public Pawn(Colors color, BoardClass board, ChessMatch match) : base(color, board)
        {
            this.match = match;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool ExistEnenmy(Position position) //if there is an opponent piece on the position
        {
            Piece piece = Board.Piece(position);
            return piece != null && piece.Color != Color;
        }

        private bool Free(Position position) // if there is no pieces on the position
        {
            return Board.Piece(position) == null;
        }

        public override bool[,] PossibleMoviments() //Implements PossibleMoviments with pawns moves
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];

            Position pos = new Position(0, 0);

            if (Color == Colors.White)
            {
                pos.DefineValues(Position.Line - 1, Position.Column);
                if (Board.VerifyPosition(pos) && Free(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 2, Position.Column);
                if (Board.VerifyPosition(pos) && Free(pos) && QuantityOfMoviments == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 1, Position.Column - 1);
                if (Board.VerifyPosition(pos) && ExistEnenmy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 1, Position.Column + 1);
                if (Board.VerifyPosition(pos) && ExistEnenmy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                // En passant
                if (Position.Line == 3)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if (Board.VerifyPosition(left) && ExistEnenmy(left) && Board.Piece(left) == match.VulnerableEnPassant)
                    {
                        mat[left.Line - 1, left.Column] = true;
                    }

                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.VerifyPosition(right) && ExistEnenmy(right) && Board.Piece(right) == match.VulnerableEnPassant)
                    {
                        mat[right.Line - 1, right.Column] = true;
                    }
                }
            }
            else
            {
                pos.DefineValues(Position.Line + 1, Position.Column);
                if (Board.VerifyPosition(pos) && Free(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line + 2, Position.Column);
                if (Board.VerifyPosition(pos) && Free(pos) && QuantityOfMoviments == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line + 1, Position.Column - 1);
                if (Board.VerifyPosition(pos) && ExistEnenmy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line + 1, Position.Column + 1);
                if (Board.VerifyPosition(pos) && ExistEnenmy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                if (Position.Line == 4)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if (Board.VerifyPosition(left) && ExistEnenmy(left) && Board.Piece(left) == match.VulnerableEnPassant)
                    {
                        mat[left.Line + 1, left.Column] = true;
                    }

                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.VerifyPosition(right) && ExistEnenmy(right) && Board.Piece(right) == match.VulnerableEnPassant)
                    {
                        mat[right.Line + 1, right.Column] = true;
                    }
                }
            }
            return mat;
        }
    }
}
