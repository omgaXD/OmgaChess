using OmgaChess.Util;

namespace OmgaChess
{
    public abstract class Board
    {
        protected int maxWidth;
        protected int maxHeight;
        public Chess Game { get; set; }

        public virtual bool W2mdef => true;
        public bool W2m = true;

        public Square[,] squares;

        public Vector2 Size
        {
            get => new Vector2(maxWidth, maxHeight);
            set {
                maxWidth = value.x;
                maxHeight = value.y;
            }
        }

        public Square this[int x, int y] => squares[x, y];
        public Square this[Vector2 v] => squares[v.x, v.y];

        protected virtual void Setup()
        {
            Size = new(maxWidth, maxHeight);
            squares = new Square[maxWidth, maxHeight];
            for (int y = 0; y < maxHeight; y++)
            {
                for (int x = 0; x < maxWidth; x++)
                {
                    squares[x, y] = new Square(this, new Vector2(x, y), null);
                }
            }
        }
        public void BroadcastMove(Piece piece, Vector2 from, Vector2 to)
        {
            foreach (Square s in squares)
            {
                if (s.Pos == to) continue;
                if (s.piece != null)
                {
                    s.piece.AfterAnyMove(piece, from, to);
                }
            }
            History.Node move = new History.Node() { info = new(), boardSize = Size, w2m = W2m};
            foreach (Square s in squares)
            {
                if (s.piece == null) continue;

                move.info.Add(s.Pos, new History.PieceData() 
                {
                    name = (s.piece.IsWhite ? s.piece.CodeName.w : s.piece.CodeName.b), 
                    data = s.piece.SaveData() , 
                    pieceType = s.piece.GetType(), 
                    white = s.piece.IsWhite
                });
            }

            Game.history.nodes.Add(move);
        }
        protected abstract void SetupPieces();
        protected abstract Vector2 DefaultSize { get; }
        public Board()
        {
            Size = DefaultSize;
            Setup();
            SetupPieces();
        }
        const int space = 0;
        public override string ToString()
        {
            string st = "";
            for (int y = maxHeight - 1; y >= 0; y--)
            {
                st += (y + 1).ToString();
                for (int i = 0; i < space; i++)
                {
                    st += ' ';
                }
                for (int x = 0; x < maxWidth; x++)
                {
                    if (squares[x, y] == null)
                    {
                        st += '0';
                    }
                    else
                    {
                        if (squares[x, y].piece == null)
                        {
                            st += '.';
                        }
                        else
                        {
                            var temp = squares[x, y].piece.DisplayChar;
                            st += squares[x, y].piece.IsWhite ? temp.w : temp.b;
                        }
                    }
                    for (int i = 0; i < space; i++)
                    {
                        st += ' ';
                    }
                }
                st += "\n";
                for (int i = 0; i < space; i++)
                {
                    st += '\n';
                }
            }
            st += " ";
            st = AddSpacing(space, st);
            for (int x = 0; x < maxWidth; x++)
            {
                st += Address.ToLiteral(x + 1);
                for (int i = 0; i < space; i++)
                {
                    st += ' ';
                }
            }
            return st + "\n";

            static string AddSpacing(int space, string st)
            {
                for (int i = 0; i < space; i++)
                {
                    st += ' ';
                }

                return st;
            }
        }
        public string SelectToString(Vector2 pos)
        {
            if (!InBounds(pos))
            {
                Console.WriteLine("Skill issue occured: address out of bounds.");
                return "";
            }
            if (this[pos].piece == null)
            {
                Console.WriteLine("Unable to select air.");
                return "";
            }
            string[] st = ToString().Split(new char[] { '\n' });
            List<Vector2> l = this[pos].piece.GetMoves();
            l = l.Select(x => new Vector2((x.x) * (space + 1) + space + 1, (maxHeight - x.y - 1) * (space + 1))).ToList();
            l.ForEach(v => {
                st[v.y] = st[v.y].Remove(v.x, 1); 
                st[v.y] = st[v.y].Insert(v.x, "@"); 
            });
            return string.Join('\n', st) + "\n";
        }
        public void HistoryLookBack(int amount)
        {
            Game.history.nodes.RemoveRange(Game.history.nodes.Count - amount, amount);
            LoadNode(Game.history.nodes.Last());
            W2m = Game.history.nodes.Last().w2m;
        }
        public void LoadNode(History.Node node)
        {
            W2m = node.w2m;
            for (int y = 0; y < maxHeight; y++)
            {
                for (int x = 0; x < maxWidth; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    if (!InBounds(pos)) continue;
                    if (!node.info.ContainsKey(pos))
                    {
                        this[pos].piece = null;
                    } else
                    {
                        Piece piece = Piece.CreateAndPutPiece(node.info[pos].pieceType, this, pos, node.info[pos].white, true);
                        piece.ReadData(node.info[pos].data);
                    }
                   
                }
            }
        }

        public virtual bool InBounds(Vector2 pos)
        {
            return (maxHeight > pos.y && maxWidth > pos.x && pos.x >= 0 && pos.y >= 0);
        }
    }
}