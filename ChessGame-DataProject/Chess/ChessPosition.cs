using System;
using ChessGame_DataProject.Board;

namespace ChessGame_DataProject.Chess
{
    class ChessPosition
    {
        public char Column { get; set; }
        public int Line { get; set; }

        public ChessPosition(char column, int line)
        {
            Column = column;
            Line = line;
        }

        // in the column case use the column - 'a' because letter a is the first letter of alphabet. 
        // if you subtract the letter a code of letter b code the result is 1, if you subtract the letter a code of letter c code the result is 2, so on.
        public Position ToPosition()
        {
            return new Position(8 - Line, Column - 'a');
        }

        public override string ToString()
        {
            return "" + Column + Line;
        }
    }
}
