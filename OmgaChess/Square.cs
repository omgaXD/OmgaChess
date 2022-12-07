using OmgaChess.Util;

namespace OmgaChess
{
    public class Square
    {
        public Board MyBoard { get; }
        public Vector2 Pos { get; }
        public Piece? piece = null;

        public Square(Board myBoard, Vector2 pos, Piece? piece = null)
        {
            MyBoard = myBoard;
            Pos = pos;
            this.piece = piece;
        }
    }
}