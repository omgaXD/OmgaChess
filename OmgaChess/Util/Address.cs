using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmgaChess.Util
{
    public struct Address
    {
        public Vector2 Pos { get; set; }

        public string Value
        {
            get
            {
                string literal = ToLiteral(Pos.x + 1);
                string num = (Pos.y + 1).ToString();
                return literal + num;
            }
            set
            {

            }
        }
        public static string ToLiteral(int decimalValue)
        {
            // Define a string variable to hold the alphabetical value
            string alphabetical = "";

            // Loop until the decimal value is less than or equal to 0
            while (decimalValue > 0)
            {
                // Calculate the current "digit" of the alphabetical value
                int digit = (decimalValue - 1) % 26;

                // Convert the digit to a character and add it to the beginning of the alphabetical value
                alphabetical = ((char)('A' + digit)).ToString() + alphabetical;

                // Divide the decimal value by 26 and round down to get the next "digit"
                decimalValue = (decimalValue - 1) / 26;
            }

            // Return the final alphabetical value
            return alphabetical;
        }
        public static int ToDecimal(string alphabetical)
        {
            // Define a variable to hold the decimal value
            int decimalValue = 0;

            // Loop through each character in the alphabetical value
            foreach (char c in alphabetical)
            {
                // Convert the character to a "digit" in base 26
                int digit = (int)(c - 'A') + 1;

                // Multiply the current decimal value by 26 and add the digit to it
                decimalValue = decimalValue * 26 + digit;
            }

            // Return the final decimal value
            return decimalValue - 1;
        }
        public Address(Vector2 v)
        {
            Pos = v;
        }
        public Address(string st)
        {
            // Define a variable to hold the literal part of the input
            string literal = "";

            // Define a variable to hold the decimal part of the input
            int decimalValue = 0;

            // Loop through each character in the input
            foreach (char c in st)
            {
                // Check if the character is a letter
                if (char.IsLetter(c))
                {
                    // Add the character to the literal part of the input
                    literal += char.ToUpperInvariant(c);
                }
                else
                {
                    // Convert the character to a decimal value and add it to the decimal part of the input
                    decimalValue = decimalValue * 10 + (int)(c - '0');
                }
            }

            // Convert the literal part of the input into a decimal value
            int literalValue = ToDecimal(literal);

            // Return the literal and decimal values as a Vector2 structure
            Pos = new Vector2(literalValue, decimalValue - 1);
        }
        public static int DigitsIn(int decimalValue)
        { 
            double logBase26 = Math.Log(decimalValue, 26);
            return (int)Math.Ceiling(logBase26);
        }
    }
}
