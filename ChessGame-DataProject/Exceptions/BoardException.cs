using System;
namespace ChessGame_DataProject.Exceptions
{
    class BoardException : Exception
    {
        public BoardException(string message) : base(message) { }
    }
}
