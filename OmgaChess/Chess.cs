using OmgaChess.Boards;
using OmgaChess.Util;

namespace OmgaChess
{
    public class Chess
    {
        public Board board;
        public History history = new() { nodes = new()};

        public bool game = false;
        public Piece selected = null;
        public bool Listen()
        {
            UserInput input = new UserInput();

            switch (input.Command)
            {
                // GOdbye
                case "quit":
                    return false;

                // Ping -> pong!
                case "ping":
                    Console.WriteLine("pong!");
                    return true;

                // Start a game (either StandartBoard or token[0])
                case "start":
                    string token = "StandartBoard";
                    if (input.Tokens.Count != 0)
                    {
                        token = input.Tokens[0];
                    }

                    Console.WriteLine($"Trying to find '{token}' board...");
                    // Use reflection to get the Type object for the class with the given name
                    Type type = Type.GetType("OmgaChess.Boards." + token);

                    // Check if the class exists and extends the Board class
                    if (type != null && type.IsSubclassOf(typeof(Board)))
                    {
                        // Create an instance of the class using Activator.CreateInstance
                        board = Activator.CreateInstance(type) as Board;
                        Console.WriteLine("Found board. Starting the game.");
                        board.W2m = board.W2mdef;
                        board.Game = this;
                        Console.WriteLine(board.ToString());
                        game = true;
                        return true;
                    } else
                    {
                        Console.WriteLine("Could not find class of needed board type.");
                    }

                    break;

                // Testing.
                case "test":
                    if (EnoughTokens(input, 1))
                    {
                        switch (input.Tokens[0])
                        {
                            case "ad":
                                try
                                {
                                    Address address = new Address(new Vector2(int.Parse(input.Tokens[1]), int.Parse(input.Tokens[2])));
                                    Console.WriteLine(address.Value);
                                    Console.WriteLine(new Address(address.Value).Value);
                                }
                                catch
                                {
                                    Console.WriteLine("Unknown skill issue occured.");
                                }
                                return true;
                            case "node":
                                Console.WriteLine(history.nodes.Last().ToString());
                                return true;
                        }
                    }
                    return true;

                // Select piece
                case "select":
                    if (TryTokenToAddress(input, 0, out Address ad)) {
                        if (TrySelectPiece(ad))
                        {
                            _Selected();
                        }
                    }
                    return true;

                // Move piece (old -> new)
                case "move":
                    if (!game)
                    {
                        _GameNotStarted();
                        return true;
                    }
                    if (EnoughTokens(input, 2))
                    {
                        Address from, to;
                        if (TryTokenToAddress(input, 0, out from) && TryTokenToAddress(input, 1, out to) && TrySelectPiece(from))
                        {
                            _MovingSelected(selected.Move(to.Pos));
                        }
                        selected = null;
                    }
                    return true;

                // Move from selected to new
                case "moveto":
                    if (EnoughTokens(input, 1))
                    {
                        if (selected != null)
                        {
                            _MovingSelected(selected.Move(new Address(input.Tokens[0].ToUpper()).Pos));
                        }
                        selected = null;
                    }
                    return true;

                // Output board
                case "board":
                    if (selected == null)
                    {
                        Console.WriteLine(board.ToString());
                    } else
                    {
                        Console.WriteLine(board.SelectToString(selected.Pos));
                    }
                    return true;
                case "goback":
                    if (EnoughTokens(input, 1, true))
                    {
                        int amount = int.Parse(input.Tokens[0]);
                        if (history.nodes.Count > amount)
                        {
                            board.HistoryLookBack(amount);
                        }
                    }
                    return true;
            }
            _UnknownCommand();
            return true;
        }

        
        

        public Chess()
        {
            while (Listen())
            {

            }
            Console.WriteLine("Quitting omgachess. Good bye.");
        }


        public bool TrySelectPiece(Address add, bool outputError = true)
        {
            if (!game)
            {
                _GameNotStarted();
            }
            if (board.InBounds(add.Pos) && board[add.Pos] != null && board[add.Pos].piece != null)
            {
                selected = board[add.Pos].piece;
                return true;
            }
            return false;
        }
        public bool TryTokenToAddress(UserInput ui, int index, out Address ad)
        {
            if (EnoughTokens(ui, index + 1, true))
            {
                ad = new Address(ui.Tokens[index]);
                return true;
            } else
            {
                ad = new Address();
                return false;
            }
        }
        public bool EnoughTokens(UserInput ui, int needed, bool outputError = true)
        {
            if (ui.Tokens.Count < needed)
            {
                if (outputError)
                {
                    _NotEnoughTokens();
                }
                return false;
            }
            return true;
        }

        private void _NotEnoughTokens() => Console.WriteLine("Not enough tokens. Try with more tokens.");
        private void _UnknownCommand() => Console.WriteLine("Unknown command.");
        private void _MovingSelected(bool result) => Console.WriteLine(result ? $"Moved {selected.GetType().Name}" : $"Can't move {selected.GetType().Name} this way");
        private void _Selected() => Console.WriteLine($"Selected {selected.GetType().Name}");

        private void _GameNotStarted() => Console.WriteLine("Start game first.");
    }
}