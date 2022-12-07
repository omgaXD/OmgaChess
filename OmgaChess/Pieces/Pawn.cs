using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmgaChess.Util;
using static OmgaChess.Util.History;

namespace OmgaChess.Pieces
{
    public class Pawn : Piece
    {
        bool canBeEnpassanted = false;
        public override (char w, char b) DisplayChar => ('p', 'P');

        public override (string w, string b) CodeName => ("p", "P");

        protected override List<Vector2> GetPossibleMoves()
        {
            int white = IsWhite ? 1 : -1;

            MoveBuilder mb = new MoveBuilder(MyBoard, this);
            mb  .AddSquare(Pos + new Vector2(1, white), MoveBuilder.Rule.OnlyTake)
                .AddSquare(Pos + new Vector2(-1, white), MoveBuilder.Rule.OnlyTake)
                .AddSquare(Pos + Vector2.up * white, MoveBuilder.Rule.NoTake);
            if ((IsWhite && Pos.y == 1) || (!IsWhite && Pos.y == MyBoard.Size.y - 2))
            {
                mb.AddSquare(Pos + (Vector2.up * 2 * white), MoveBuilder.Rule.NoTake);
            }
            if (MyBoard.InBounds(Pos + Vector2.right) && MyBoard[Pos + Vector2.right].piece is Pawn p1)
            {
                if (p1.canBeEnpassanted && p1.IsWhite != IsWhite)
                {
                    mb.AddSquare(Pos +  new Vector2(1, white), MoveBuilder.Rule.NoTake);
                }
            }
            if (MyBoard.InBounds(Pos + Vector2.left) && MyBoard[Pos + Vector2.left].piece is Pawn p2)
            {
                if (p2.canBeEnpassanted && p2.IsWhite != IsWhite)
                {
                    mb.AddSquare(Pos + new Vector2(-1, white), MoveBuilder.Rule.NoTake);
                }
            }
            return mb.Build();
        }

        protected override void AfterMove(Vector2 oldPos, Vector2 newPos)
        {
            if (Math.Abs(oldPos.y - newPos.y) == 2)
            {
                canBeEnpassanted = true;
            }
        }
        protected override void OnMove(Vector2 oldPos, Vector2 newPos)
        {
            int white = IsWhite ? 1 : -1;
            Vector2 pos = newPos + Vector2.down * white;
            if (MyBoard.InBounds(pos) && 
                MyBoard[pos].piece is Pawn p && 
                MyBoard[newPos].piece == null && 
                p.canBeEnpassanted && Math.Abs(oldPos.x - newPos.x) == 1)
            {
                MyBoard[pos].piece = null;
            }
        }
        public override void AfterAnyMove(Piece movedPiece, Vector2 oldPos, Vector2 newPos)
        {
            canBeEnpassanted = false;
        }

        public override Dictionary<string, int>? SaveData()
        {
            return new() { { "e", (canBeEnpassanted ? 1 : 0) } };
        }

        public override void ReadData(Dictionary<string, int>? dic)
        {
            canBeEnpassanted = dic["e"] == 1;
        }
    }
    
}

