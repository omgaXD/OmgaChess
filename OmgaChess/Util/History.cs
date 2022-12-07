namespace OmgaChess.Util
{
    public struct History
    {
        public List<Node> nodes;
        public static implicit operator List<Node>(History h) => h.nodes;
        public struct Node
        {
            public bool w2m;
            public Vector2 boardSize;
            public Dictionary<Vector2, PieceData> info;
            public override string ToString()
            {
                string st = $"({boardSize.x},{boardSize.y}):";
                PieceData?[,] arr = new PieceData?[boardSize.x, boardSize.y];
                for (int x = 0; x < boardSize.x; x++)
                {
                    for (int y = 0; y < boardSize.y; y++)
                    {
                        arr[x, y] = null;
                    }
                }
                info.ToList().ForEach(kvp =>
                {
                    arr[kvp.Key.x, kvp.Key.y] = kvp.Value;
                });

                int nullCounter = 0;
                for (int y = 0; y < boardSize.y; y++)
                {
                    for (int x = 0; x < boardSize.x; x++)
                    {
                        if (arr[x, y] == null)
                        {
                            nullCounter++;
                        } else
                        {
                            if (nullCounter > 0)
                            {
                                st += nullCounter;
                                nullCounter = 0;
                            }
                            st += arr[x, y].Value.name;
                            if (arr[x,y].Value.data != null)
                            {
                                st += "{";
                                foreach (var item in arr[x, y].Value.data)
                                {
                                    st += $"{item.Key}={item.Value},";
                                }
                                st = st.Substring(0, st.Length - 1); 
                                st += "}";
                            }
                        }
                    }
                }

                return st;
            }
        }
        public struct PieceData
        {
            public string name;
            public Type pieceType;
            public bool white;
            public Dictionary<string, int> data;
        }


    }

}

