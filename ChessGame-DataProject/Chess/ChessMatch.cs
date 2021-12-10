using System;
using System.Collections.Generic;
using ChessGame_DataProject.Board;
using ChessGame_DataProject.Exceptions;
using ChessGame_DataProject.Pieces;

namespace ChessGame_DataProject.Chess
{
    class ChessMatch
    {
        public BoardClass Board { get; private set; }
        public int Turn { get;  set; }
        public Colors CurrentPlayer { get; private set; }
        public bool Finish { get; private set; }
        private HashSet<Piece> pieces;
        private HashSet<Piece> capturedPieces;
        public double Advantage { get; set; }
        public Piece VulnerableEnPassant { get; private set; }
        public bool Check { get; private set; }
        public int[] golds = new int[2];
        public int[] Golds { get { return golds; } set { golds = value; } }
        public int[] gains = new int[2];
        public int[] Gains { get { return gains; } set { gains = value; } }

        public ChessMatch()
        {
            Board = new BoardClass(8, 8);
            Turn = 1;
            CurrentPlayer = Colors.White;
            Finish = false;
            Check = false;
            pieces = new HashSet<Piece>();
            capturedPieces = new HashSet<Piece>();
            VulnerableEnPassant = null;
            PutingInitialPieces();
            golds[0] = 0;
            golds[1] = 0;
            gains[0] = 1;
            gains[1] = 1;
        }
        public static void KnowAdvantage(ChessMatch match) //AMELIORER CE PROGRAMME
        {
            double Wscore=0;
            double Wweaknesses = 0;
            double Wstrenghts = 0;
            double Bscore = 0;
            double Bweaknesses = 0;
            double Bstrenghts = 0;
            foreach (Piece p in match.capturedPieces)
            {
                if (p.Color==Colors.Black)
                {
                    if (p.ToString() == "B" || p.ToString()=="H") //piece is a Bishop or a Horse
                    {
                        Wscore += 3;
                    }
                    else if (p.ToString() == "R") //piece is a Rook
                    {
                        Wscore += 5;
                    }
                    else if (p.ToString() == "Q") //piece is a Queen
                    {
                        Wscore += 9;
                    }
                    else //piece is a Pawn
                    {
                        Wscore += 1;
                    }
                }
                else
                {
                    if (p.ToString() == "B" || p.ToString() == "H") //piece is a Bishop or a Horse
                    {
                        Bscore += 3;
                    }
                    else if (p.ToString() == "R") //piece is a Rook
                    {
                        Bscore += 5;
                    }
                    else if (p.ToString() == "Q") //piece is a Queen
                    {
                        Bscore += 9;
                    }
                    else //piece is a Pawn
                    {
                        Bscore += 1;
                    }
                }
                //For white
                for (int column = 0; column < 8; column++) //double pawns
                {
                    int auxiliar = -1;
                    for (int line = 0; line < 8; line++)
                    {
                        if (match.Board.Piece(line, column)!=null)
                        {
                            if (match.Board.Piece(line, column).Color == Colors.White && match.Board.Piece(line, column).ToString() == "P")
                            {
                                auxiliar += 1;
                            }
                        }
                    }
                    if (auxiliar >= 2)
                    {
                        Wweaknesses += (auxiliar-1) * 0.25;
                    }
                }
                if (match.Board.Piece(5, 5) != null)
                {
                    if (match.Board.Piece(5, 5).Color == Colors.White) //control of the center
                    {
                        Wstrenghts += 0.2;
                    }
                }
                if (match.Board.Piece(4, 4) != null)
                {
                    if (match.Board.Piece(4, 4).Color == Colors.White)
                    {
                        Wstrenghts += 0.3;
                    }
                }
                if (match.Board.Piece(5, 4) != null)
                {
                    if (match.Board.Piece(5, 4).Color == Colors.White)
                    {
                        Wstrenghts += 0.2;
                    }
                }
                if (match.Board.Piece(4, 5) != null)
                {
                    if (match.Board.Piece(4, 5).Color == Colors.White)
                    {
                        Wstrenghts += 0.3;
                    }
                }
                //For black
                for (int column = 0; column < 8; column++) //double pawns
                {
                    int auxiliar = 0;
                    for (int line = 0; line < 8; line++)
                    {
                        if (match.Board.Piece(line, column) != null)
                        {
                            if (match.Board.Piece(line, column).Color == Colors.Black && match.Board.Piece(line, column).ToString() == "P")
                            {
                                auxiliar += 1;
                            }
                        }
                    }
                    if (auxiliar>=2)
                    {
                        Bweaknesses += (auxiliar-1) * 0.25;
                    }
                }
                if (match.Board.Piece(5, 5) != null)
                {
                    if (match.Board.Piece(5, 5).Color == Colors.Black) //control of the center
                    {
                        Bstrenghts += 0.3;
                    }
                }
                if (match.Board.Piece(4, 4) != null)
                {
                    if (match.Board.Piece(4, 4).Color == Colors.Black)
                    {
                        Bstrenghts += 0.2;
                    }
                }
                if (match.Board.Piece(5, 4) != null)
                {
                    if (match.Board.Piece(5, 4).Color == Colors.Black)
                    {
                        Bstrenghts += 0.3;
                    }
                }
                if (match.Board.Piece(4, 5) != null)
                {
                    if (match.Board.Piece(4, 5).Color == Colors.Black)
                    {
                        Bstrenghts += 0.2;
                    }
                }
            }
            Wscore = Wscore-Wweaknesses+Wstrenghts;
            Bscore = Bscore-Bweaknesses+Bstrenghts;
            match.Advantage = Wscore - Bscore;
        }
        public static void EarnGold(ChessMatch match)
        {
            if (match.CurrentPlayer==Colors.White)
            {
                match.Golds[0] += match.Gains[0];
                Console.WriteLine($"You earned {match.Gains[0]} golds");
                Console.WriteLine();
            }
            else
            {
                match.Golds[1] += match.Gains[1];
                Console.WriteLine($"You earned {match.Gains[1]} golds");
                Console.WriteLine();
            }
        }
        public static void BuyBuilding (ChessMatch match)
        {
            if (match.CurrentPlayer == Colors.White)
            {
                Console.WriteLine("Which building do you want to buy?");
                Console.WriteLine("Inside market: Cost=3 : Gains=1 : Type 1");
                Console.WriteLine("Apartement: Cost=5 : Gains=2 : Type 2");
                Console.WriteLine("Hotel: Cost=9 : Gains=4 : Type 3");
                Console.WriteLine("Bank: Cost=12 : Gains=6 : Type 4");
                Console.WriteLine("Change your mind and earn golds : Type 5");
                string building = Console.ReadLine();
                Console.WriteLine();
                if (building=="1")
                {
                    Console.WriteLine();
                    if (match.Golds[0]>=3) //check if you have enough golds
                    {
                        match.Golds[0] -= 3; //buy
                        match.Gains[0] += 1; //increase our gains
                        Console.WriteLine("New inside market bought");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money");
                        BuyBuilding(match);
                        Console.WriteLine();
                            
                    }
                }
                else if (building=="2")
                {
                    Console.WriteLine();
                    if (match.Golds[0] >= 5)
                    {
                        match.Golds[0] -= 5;
                        match.Gains[0] += 2;
                        Console.WriteLine("New apartement bought");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money");
                        BuyBuilding(match);
                        Console.WriteLine();

                    }
                }
                else if (building == "3")
                {
                    Console.WriteLine();
                    if (match.Golds[0] >= 9)
                    {
                        match.Golds[0] -= 9;
                        match.Gains[0] += 4;
                        Console.WriteLine("New hotel bought");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money");
                        BuyBuilding(match);
                        Console.WriteLine();

                    }
                }
                else if (building == "4")
                {
                    Console.WriteLine();
                    if (match.Golds[0] >= 12)
                    {
                        match.Golds[0] -= 12;
                        match.Gains[0] += 6;
                        Console.WriteLine("New bank bought");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money");
                        BuyBuilding(match);
                        Console.WriteLine();

                    }
                }
                else if (building=="5")
                {
                    EarnGold(match);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Sorry there is an error. PLease try again");
                    BuyBuilding(match);
                    Console.WriteLine();

                }
            }
            else
            {
                Console.WriteLine("Which building do you want to buy?");
                Console.WriteLine("Inside market: Cost=3 : Gains=1 : Type 1");
                Console.WriteLine("Apartement: Cost=5 : Gains=2 : Type 2");
                Console.WriteLine("Hotel: Cost=9 : Gains=4 : Type 3");
                Console.WriteLine("Bank: Cost=12 : Gains=6 : Type 4");
                Console.WriteLine("Change your mind and earn golds : Type 5");
                string building = Console.ReadLine();
                Console.WriteLine();
                if (building == "1")
                {
                    Console.WriteLine();
                    if (match.Golds[1] >= 3)
                    {
                        match.Golds[1] -= 3;
                        match.Gains[1] += 1;
                        Console.WriteLine("New inside market bought");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money");
                        BuyBuilding(match);
                        Console.WriteLine();

                    }
                }
                else if (building == "2")
                {
                    Console.WriteLine();
                    if (match.Golds[1] >= 5)
                    {
                        match.Golds[1] -= 5;
                        match.Gains[1] += 2;
                        Console.WriteLine("New apartement bought");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money");
                        BuyBuilding(match);
                        Console.WriteLine();

                    }
                }
                else if (building == "3")
                {
                    Console.WriteLine();
                    if (match.Golds[1] >= 9)
                    {
                        match.Golds[1] -= 9;
                        match.Gains[1] += 4;
                        Console.WriteLine("New hotel bought");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money");
                        BuyBuilding(match);
                        Console.WriteLine();

                    }
                }
                else if (building == "4")
                {
                    Console.WriteLine();
                    if (match.Golds[1] >= 12)
                    {
                        match.Golds[1] -= 12;
                        match.Gains[1] += 6;
                        Console.WriteLine("New bank bought");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough money");
                        BuyBuilding(match);
                        Console.WriteLine();

                    }
                }
                else if (building == "5")
                {
                    EarnGold(match);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Sorry there is an error. PLease try again");
                    BuyBuilding(match);
                    Console.WriteLine();
                }
            }
        }
        public static void TwoMoves(ChessMatch match)
        {
            if (match.CurrentPlayer == Colors.White)
            {
                if (match.Golds[0]>=5)
                {
                    match.Golds[0] -= 5;
                    Console.WriteLine("You paid 5 golds to play twice");
                    Console.WriteLine();
                    Console.WriteLine("First move");
                    Console.WriteLine();

                    Console.Write("Origin: "); // Just repeat the instructions to make a move but twice
                    Position origin1 = Screen.ReadChessPosition().ToPosition();
                    match.ValidOriginPosition(origin1);

                    bool[,] possiblePositions1 = match.Board.Piece(origin1).PossibleMoviments();

                    Console.Clear();
                    Screen.PrintBoard(match.Board, possiblePositions1);

                    Console.WriteLine("\n");

                    Console.Write("Destiny: ");
                    Position destiny1 = Screen.ReadChessPosition().ToPosition();
                    match.ValidDestinyPosition(origin1, destiny1);

                    match.RealizeMove(origin1, destiny1);
                    match.ChangePlayer();
                    match.Turn--;
                    
                    Screen.PrintMatch(match);
                    Console.WriteLine();
                    Console.WriteLine("Second move");
                    Console.WriteLine();

                    Console.Write("Origin: "); // Second time
                    Position origin2 = Screen.ReadChessPosition().ToPosition();
                    match.ValidOriginPosition(origin2);

                    bool[,] possiblePositions2 = match.Board.Piece(origin2).PossibleMoviments();

                    Console.Clear();
                    Screen.PrintBoard(match.Board, possiblePositions2);

                    Console.WriteLine("\n");

                    Console.Write("Destiny: ");
                    Position destiny2 = Screen.ReadChessPosition().ToPosition();
                    match.ValidDestinyPosition(origin2, destiny2);

                    match.RealizeMove(origin2, destiny2);

                }
                else
                {
                    Console.WriteLine("You don't have 5 golds so you earned golds instead");
                    EarnGold(match);
                }
            }
            else
            {
                if (match.Golds[1] >= 5)
                {
                    match.Golds[1] -= 5;
                    Console.WriteLine("You paid 5 golds to play twice");
                    Console.WriteLine();
                    Console.WriteLine("First move");
                    Console.WriteLine();

                    Console.Write("Origin: ");
                    Position origin1 = Screen.ReadChessPosition().ToPosition();
                    match.ValidOriginPosition(origin1);

                    bool[,] possiblePositions1 = match.Board.Piece(origin1).PossibleMoviments();

                    Console.Clear();
                    Screen.PrintBoard(match.Board, possiblePositions1);

                    Console.WriteLine("\n");

                    Console.Write("Destiny: ");
                    Position destiny1 = Screen.ReadChessPosition().ToPosition();
                    match.ValidDestinyPosition(origin1, destiny1);

                    match.RealizeMove(origin1, destiny1);
                    match.ChangePlayer();
                    match.Turn--;

                    Console.WriteLine();
                    Console.WriteLine("Second move");
                    Console.WriteLine();

                    Console.Write("Origin: ");
                    Position origin2 = Screen.ReadChessPosition().ToPosition();
                    match.ValidOriginPosition(origin2);

                    bool[,] possiblePositions2 = match.Board.Piece(origin2).PossibleMoviments();

                    Console.Clear();
                    Screen.PrintBoard(match.Board, possiblePositions2);

                    Console.WriteLine("\n");

                    Console.Write("Destiny: ");
                    Position destiny2 = Screen.ReadChessPosition().ToPosition();
                    match.ValidDestinyPosition(origin2, destiny2);

                    match.RealizeMove(origin2, destiny2);

                }
                else
                {
                    Console.WriteLine("You don't have 5 golds so you earned golds instead");
                    EarnGold(match);
                }
            }

        }
        public static void AtomicMissile(ChessMatch match) //This is quite similar than in Call of Duty when you make a nuke, you win directly
        {
            if (match.CurrentPlayer==Colors.White)
            {
                if (match.Golds[0]>=20)
                {
                    Console.WriteLine("The atomic missile has been launched. You won!");
                    match.Golds[0] -= 20;
                    match.Finish = true;
                }
                else
                {
                    Console.WriteLine("You don't have 20 golds so you earned golds instead");
                    EarnGold(match);
                }
            }
            else
            {
                if (match.Golds[1] >= 20)
                {
                    Console.WriteLine("The atomic missile has been launched. You won!");
                    match.Golds[1] -= 20;
                    match.Finish = true;
                }
                else
                {
                    Console.WriteLine("You don't have 20 golds so you earned golds instead");
                    EarnGold(match);
                }
            }
        }

        public Piece ExecutMoviment(Position origin, Position destiny)
        {
            Piece piece = Board.RemovePiece(origin);
            piece.IncrementMovimentsQuantity();
            Piece capturedPiece = Board.RemovePiece(destiny);
            Board.PutPiece(piece, destiny);
            if (capturedPiece != null)
            {
                capturedPieces.Add(capturedPiece);
            }

            // Especial move small roque
            if (piece is King && destiny.Column == origin.Column + 2)
            {
                Position RookOrigin = new Position(origin.Line, origin.Column + 3);
                Position RookDestiny = new Position(origin.Line, origin.Column + 1);// Small roque is the right side 
                Piece rook = Board.RemovePiece(RookOrigin);
                rook.IncrementMovimentsQuantity(); 
                Board.PutPiece(rook, RookDestiny);
            }

            // Especial move big roque
            if (piece is King && destiny.Column == origin.Column - 2)
            {
                Position RookOrigin = new Position(origin.Line, origin.Column - 4);
                Position RookDestiny = new Position(origin.Line, origin.Column - 1); // Big roque is the left side
                Piece rook = Board.RemovePiece(RookOrigin);
                rook.IncrementMovimentsQuantity();
                Board.PutPiece(rook, RookDestiny);
            }

            // Especial move en passant
            if (piece is Pawn)
            {
                if (origin.Column != destiny.Column && capturedPiece == null)
                {
                    Position pawnPosition;
                    if (piece.Color == Colors.White)
                    {
                        pawnPosition = new Position(destiny.Line + 1, destiny.Column);
                    }
                    else
                    {
                        pawnPosition = new Position(destiny.Line - 1, destiny.Column);
                    }
                    capturedPiece = Board.RemovePiece(pawnPosition);
                    capturedPieces.Add(capturedPiece);
                }
            }

            return capturedPiece;
        }


        public void RealizeMove(Position origin, Position destiny)
        {
            Piece capturedPiece = ExecutMoviment(origin, destiny);
            if (IsInCheck(CurrentPlayer)) //to be sure it's not a pinned piece
            {
                UndoMoviment(origin, destiny, capturedPiece); //cancel the moviment if it was
                throw new BoardException("You can't put yourself in check");
            }

            Piece piece = Board.Piece(destiny);

            // Especial move promotion
            if (piece is Pawn)
            {
                if ((piece.Color == Colors.White && destiny.Line == 0) || (piece.Color == Colors.Black && destiny.Line == 7))
                {
                    piece = Board.RemovePiece(destiny);
                    pieces.Remove(piece);

                    Piece queen = new Queen(piece.Color, Board); //Auto promotion to queen to make that easier otherwise we can ask the player which type of piece he wants
                    Board.PutPiece(queen, destiny);
                    pieces.Add(queen);
                }
            }

            if (IsInCheck(Opponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }

            if (TestCheckmate(Opponent(CurrentPlayer)))
            {
                Finish = true;
            }
            else
            {
                Turn++;
                ChangePlayer();
            }

            // Especial move - en passant
            if (piece is Pawn && (destiny.Line == origin.Line - 2 || destiny.Line == origin.Line + 2))
            {
                VulnerableEnPassant = piece; // to store the vulnerable en passant piece
            }
            else
            {
                VulnerableEnPassant = null; // reset if there is not
            }
        }

        public void UndoMoviment(Position origin, Position destiny, Piece capturedPiece)// Cancel the move so it does the reverse actions to ExecutMoviment
        {
            Piece piece = Board.RemovePiece(destiny);
            piece.DecrementMovimentsQuantity();
            if (capturedPiece != null)
            {
                Board.PutPiece(capturedPiece, destiny);
                capturedPieces.Remove(capturedPiece);
            }
            Board.PutPiece(piece, origin);

            // Especial move small roque
            if (piece is King && destiny.Column == origin.Column + 2)
            {
                Position RookOrigin = new Position(origin.Line, origin.Column + 3);
                Position RookDestiny = new Position(origin.Line, origin.Column + 1);
                Piece rook = Board.RemovePiece(RookDestiny);
                rook.DecrementMovimentsQuantity();
                Board.PutPiece(rook, RookOrigin);
            }

            // Especial move big roque
            if (piece is King && destiny.Column == origin.Column - 2)
            {
                Position RookOrigin = new Position(origin.Line, origin.Column - 4);
                Position RookDestiny = new Position(origin.Line, origin.Column - 1);
                Piece rook = Board.RemovePiece(RookDestiny);
                rook.DecrementMovimentsQuantity();
                Board.PutPiece(rook, RookOrigin);
            }

            // Especial move en passant
            if (piece is Pawn)
            {
                if (origin.Column != destiny.Column && capturedPiece == VulnerableEnPassant)
                {
                    Piece pawn = Board.RemovePiece(destiny);
                    Position pawnPosition;
                    if (piece.Color == Colors.White)
                    {
                        pawnPosition = new Position(3, destiny.Column);
                    }
                    else
                    {
                        pawnPosition = new Position(4, destiny.Column);
                    }
                    Board.PutPiece(pawn, pawnPosition);
                }
            }
        }

        public void ValidOriginPosition(Position position) //Throw basics BoardExceptions
        {
            if (Board.Piece(position) == null)
            {
                throw new BoardException("Not exist piece in the origin position!");
            }
            if (CurrentPlayer != Board.Piece(position).Color)
            {
                throw new BoardException("The origin piece chosen for you it's not yours!");
            }
            if (!Board.Piece(position).ExistPossibleMoviments())
            {
                throw new BoardException("Don't have possible moviments for the origin piece chosed!");
            }
        }

        public void ValidDestinyPosition(Position origin, Position destiny)
        {
            if (!Board.Piece(origin).CanMoveTo(destiny)) // if the destiny is a false in the 2 dimensionnal array PossibleMoviments
            {
                throw new BoardException("Invalid destiny position!");
            }
        }

        public void ChangePlayer()
        {
            if (CurrentPlayer == Colors.White)
            {
                CurrentPlayer = Colors.Black;
            }
            else
            {
                CurrentPlayer = Colors.White;
            }
        }

        public HashSet<Piece> CapturedPiece(Colors color) //build the HashSet of capturedPieces
        {
            HashSet<Piece> auxiliar = new HashSet<Piece>();
            foreach (Piece piece in capturedPieces)
            {
                if (piece.Color == color)
                {
                    auxiliar.Add(piece);
                }
            }
            return auxiliar;
        }

        public HashSet<Piece> PiecesInGame(Colors color) //build the HashSet of pieces
        {
            HashSet<Piece> auxiliar = new HashSet<Piece>();
            foreach (Piece piece in pieces)
            {
                if (piece.Color == color)
                {
                    auxiliar.Add(piece);
                }
            }
            auxiliar.ExceptWith(CapturedPiece(color));
            return auxiliar;
        }

        private Piece King(Colors color) //return the king of the choosen player
        {
            foreach (Piece piece in PiecesInGame(color))
            {
                if (piece is King)
                {
                    return piece;
                }
            }
            return null;
        }

        public bool IsInCheck(Colors color)
        {
            Piece king = King(color);
            if (king == null)
            {
                throw new BoardException($"Don't have {color} king in the board!");
            }
            foreach (Piece piece in PiecesInGame(Opponent(color))) // build all moviments the opponent will be able to do in the next turn
            {
                bool[,] mat = piece.PossibleMoviments();
                if (mat[king.Position.Line, king.Position.Column])// This check if a piece can attack the king
                {
                    return true;
                }
            }
            return false;
        }

        public bool TestCheckmate(Colors color)
        {
            if (!IsInCheck(color))
            {
                return false;
            }

            foreach (Piece piece in PiecesInGame(color))
            {
                bool[,] mat = piece.PossibleMoviments();

                for (int i = 0; i < Board.Lines; i++)
                {
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origin = piece.Position;
                            Position destiny = new Position(i, j);
                            Piece capturedPiece = ExecutMoviment(origin, destiny); //test if we can either move our king of do a move to protect our king
                            bool testCheck = IsInCheck(color);
                            UndoMoviment(origin, destiny, capturedPiece);

                            if (!testCheck)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private Colors Opponent(Colors color)
        {
            if (color == Colors.White)
            {
                return Colors.Black;
            }
            else
            {
                return Colors.White;
            }
        }

        public void PutNewPiece(char column, int line, Piece piece)
        {
            Board.PutPiece(piece, new ChessPosition(column, line).ToPosition());
            pieces.Add(piece);
        }

        public void PutingInitialPieces() //Initializing the board
        {
            PutNewPiece('a', 1, new Rook(Colors.White, Board));
            PutNewPiece('b', 1, new Horse(Colors.White, Board));
            PutNewPiece('c', 1, new Bishop(Colors.White, Board));
            PutNewPiece('d', 1, new Queen(Colors.White, Board));
            PutNewPiece('e', 1, new King(Colors.White, Board, this));
            PutNewPiece('f', 1, new Bishop(Colors.White, Board));
            PutNewPiece('g', 1, new Horse(Colors.White, Board));
            PutNewPiece('h', 1, new Rook(Colors.White, Board));
            PutNewPiece('a', 2, new Pawn(Colors.White, Board, this));
            PutNewPiece('b', 2, new Pawn(Colors.White, Board, this));
            PutNewPiece('c', 2, new Pawn(Colors.White, Board, this));
            PutNewPiece('d', 2, new Pawn(Colors.White, Board, this));
            PutNewPiece('e', 2, new Pawn(Colors.White, Board, this));
            PutNewPiece('f', 2, new Pawn(Colors.White, Board, this));
            PutNewPiece('g', 2, new Pawn(Colors.White, Board, this));
            PutNewPiece('h', 2, new Pawn(Colors.White, Board, this));

            PutNewPiece('a', 8, new Rook(Colors.Black, Board));
            PutNewPiece('b', 8, new Horse(Colors.Black, Board));
            PutNewPiece('c', 8, new Bishop(Colors.Black, Board));
            PutNewPiece('d', 8, new Queen(Colors.Black, Board));
            PutNewPiece('e', 8, new King(Colors.Black, Board, this));
            PutNewPiece('f', 8, new Bishop(Colors.Black, Board));
            PutNewPiece('g', 8, new Horse(Colors.Black, Board));
            PutNewPiece('h', 8, new Rook(Colors.Black, Board));
            PutNewPiece('a', 7, new Pawn(Colors.Black, Board, this));
            PutNewPiece('b', 7, new Pawn(Colors.Black, Board, this));
            PutNewPiece('c', 7, new Pawn(Colors.Black, Board, this));
            PutNewPiece('d', 7, new Pawn(Colors.Black, Board, this));
            PutNewPiece('e', 7, new Pawn(Colors.Black, Board, this));
            PutNewPiece('f', 7, new Pawn(Colors.Black, Board, this));
            PutNewPiece('g', 7, new Pawn(Colors.Black, Board, this));
            PutNewPiece('h', 7, new Pawn(Colors.Black, Board, this));
        }
    }
}
