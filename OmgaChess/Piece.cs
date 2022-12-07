using OmgaChess.Util;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace OmgaChess
{
    public abstract class Piece
    {
        public Board MyBoard { get; private set; }
        protected Vector2 pos;
        public Vector2 Pos { get => pos; set => Move(value); }
        public bool IsWhite { get; private set; }

        protected virtual void OnMove(Vector2 oldPos, Vector2 newPos)
        {
            return;
        }
        protected virtual void AfterMove(Vector2 oldPos, Vector2 newPos)
        {
            return;
        }
        public virtual void AfterAnyMove(Piece movedPiece, Vector2 oldPos, Vector2 newPos)
        {
            return;
        }

        public bool Move(Vector2 to) {
            if (!GetMoves().Contains(to))
            {
                return false;
            }

            Vector2 old = pos;
            OnMove(old, to);
            SilentMove(to);
            AfterMove(old, to);
            MyBoard.BroadcastMove(this, old, to);
            MyBoard.W2m = !IsWhite;
            return true;
        }
        protected void SilentMove(Vector2 to)
        {
            Vector2 old = pos;
            pos = to;
            MyBoard[old].piece = null;
            MyBoard[to].piece = this;
        }
        public List<Vector2> GetMoves()
        {
            if (MyBoard.W2m != IsWhite)
            {
                return new List<Vector2>();
            } else
            {
                return GetPossibleMoves();
                // check/mate code goes here
            }
        }
        protected abstract List<Vector2> GetPossibleMoves();
        public abstract (char w, char b) DisplayChar { get; }
        public abstract (string w, string b) CodeName { get; }

        public static T CreateAndPutPiece<T>(Board board, Vector2 pos, bool white = true, bool force = true) where T : Piece
        {
            if (board == null) throw new NullReferenceException();
            if (board[pos] == null) throw new NullReferenceException();
            
            T piece = (T)Activator.CreateInstance(typeof(T));
            if (piece == null) throw new Exception("Can't create the piece wtf");

            board[pos].piece = piece;
            piece.MyBoard = board;
            piece.pos = pos;
            piece.IsWhite = white;

            return piece;
        }
        public static Piece CreateAndPutPiece(Type type, Board board, Vector2 pos, bool white = true, bool force = true)
        {
            MethodInfo method = typeof(Piece).GetMethods().ToList().Find(mi => mi.IsGenericMethod && mi.Name == "CreateAndPutPiece");
            MethodInfo genericMethod = method.MakeGenericMethod(type);
            return (Piece)genericMethod.Invoke(null, new object[] { board, pos, white, force });
        }
        protected Piece()
        {
        }

        public virtual Dictionary<string, int>? SaveData()
        {
            return null;
        }
        public virtual void ReadData(Dictionary<string, int>? st)
        {

        }
    }
}