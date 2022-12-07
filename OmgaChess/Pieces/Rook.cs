using OmgaChess.Util;

namespace OmgaChess.Pieces
{
    public class Rook : Piece
    {
        public override (char w, char b) DisplayChar => ('r', 'R');

        public override (string w, string b) CodeName => ("r", "R");

        protected override List<Vector2> GetPossibleMoves()
        {
            MoveBuilder mb = new MoveBuilder(MyBoard, this);
            foreach (Vector2 dir in Vector2.verHoz)
            {
                mb.AddLine(Pos, dir);
            }
            
            return mb.Build();
        }
    }
}

