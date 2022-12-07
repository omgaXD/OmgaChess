namespace OmgaChess.Util
{
    public class UserInput
    {
        public string Command { get; set; }
        public List<string> Tokens { get; set; }
        public UserInput()
        {
            string input = Console.ReadLine();

            // Split the input string into tokens separated by whitespace
            string[] tokens = input.Split(' ');

            // The first token is the command
            string command = tokens[0].ToLower();

            // Remove the first token (the command) from the list of tokens
            List<string> remainingTokens = new List<string>(tokens);
            remainingTokens.RemoveAt(0);

            Command = command;
            Tokens = remainingTokens;
        }
        public static UserInput Read()
        {
            // Read the input string from the console
            string input = Console.ReadLine();

            // Split the input string into tokens separated by whitespace
            string[] tokens = input.Split(' ');

            // The first token is the command
            string command = tokens[0];

            // Remove the first token (the command) from the list of tokens
            List<string> remainingTokens = new List<string>(tokens);
            remainingTokens.RemoveAt(0);

            // Create a new UserInput instance with the command and remaining tokens
            return new UserInput
            {
                Command = command,
                Tokens = remainingTokens
            };
        }
    }
}