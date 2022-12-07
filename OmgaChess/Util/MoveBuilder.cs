namespace OmgaChess.Util
{
    public class MoveBuilder
    {
        public bool white { get; private set; }
        public Board Board { get; private set; }
        public MoveBuilder(Board board, Piece p)
        {
            Board = board;
            this.white = p.IsWhite;
            removed.Add(p.Pos);
        }

        List<Vector2> possibleMoves = new List<Vector2>();
        List<Vector2> removed = new List<Vector2>();
        public MoveBuilder AddSquare(Vector2 pos, Rule rule)
        {
            if (!Board.InBounds(pos))
            {
                return this;
            }
            Square sq = Board[pos];
            if (sq == null) return this;
            if (rule.ontoEmpty && sq.piece == null) possibleMoves.Add(pos);
            if (sq.piece == null) return this;
            if (rule.ontoEnemy && sq.piece.IsWhite != white) { possibleMoves.Add(pos); }
            if (rule.ontoOwn && sq.piece.IsWhite == white) { possibleMoves.Add(pos); }

            return this;
        }
        public MoveBuilder AddLine(Vector2 origin, Vector2 direction, Rule? ruleOrNull = null)
        {
            Rule rule = ruleOrNull ?? Rule.Default;
            while (Board.InBounds(origin + direction))
            {
                origin += direction;
                AddSquare(origin, rule);
                Square sq = Board[origin];
                if (sq == null) return this;
                
                if (sq.piece == null)
                {
                    if (!rule.continueAfterEmpty) return this;
                    continue;
                }
                if (sq.piece.IsWhite != white)
                {
                    if (!rule.continueAfterEnemy) return this;
                    continue;
                }
                if (sq.piece.IsWhite == white)
                {
                    if (!rule.continueAfterOwn) return this;
                    continue;
                }
            }
            return this;
        }
        public List<Vector2> Build()
        {
            return possibleMoves.Distinct().ToList().FindAll(v => !removed.Contains(v));
        }
        public struct Rule
        {
            public bool ontoOwn = false;
            public bool ontoEnemy = true;
            public bool ontoEmpty = true;
            public bool continueAfterEnemy = false;
            public bool continueAfterOwn = false;
            public bool continueAfterEmpty = true;

            public Rule()
            {
            }

            public static Rule NoTake => new Rule() { ontoEnemy = false };
            public static Rule Default => new Rule();
            public static Rule OnlyTake => new Rule() { ontoEmpty = false };
        }
    }
}