using OmgaChess.Pieces;
using OmgaChess.Util;

namespace OmgaChess.Boards
{
    public class StandartBoard : Board
    {

        protected override Vector2 DefaultSize => new(8, 8);

        protected override void SetupPieces()
        {
            for (int x = 0; x < 8; x++)
            {
                Piece.CreateAndPutPiece<Pawn>(this, new Vector2(x, 1));
                Piece.CreateAndPutPiece<Pawn>(this, new Vector2(x, 6), false);
                if (x == 0 || x == 7)
                {
                    Piece.CreateAndPutPiece<Rook>(this, new Vector2(x, 0));
                    Piece.CreateAndPutPiece<Rook>(this, new Vector2(x, 7), false);
                }
            }
        }
    }
}