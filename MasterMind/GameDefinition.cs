namespace MasterMind
{
    public class GameDefinition : IGameDefinition
    {
        public int AllowedGuesses { get; private set; }
        public int GuessLength { get; private set; }
        public int DigitMinVal { get; private set; }
        public int DigitMaxVal { get; private set; }

        public GameDefinition(int allowedGuesses, int guessLength, int digitMinVal, int digitMaxVal)
        {
            AllowedGuesses = allowedGuesses;
            GuessLength = guessLength;
            DigitMinVal = digitMinVal;
            DigitMaxVal = digitMaxVal;
        }

        public bool ValidateInputFormat(string guess, out string message)
        {
            message = string.Empty;
            const string messageSuffix = ", try again";
            if (string.IsNullOrWhiteSpace(guess))
            {
                message = $"Empty input{messageSuffix}";
                return false;
            }
            if (guess.Length != GuessLength)
            {
                message = $"Must enter {GuessLength} digits{messageSuffix}";
                return false;
            }
            foreach(var chr in guess)
            {
                if (!int.TryParse(chr.ToString(), out var digit))
                {
                    message = $"Each character must be a digit{messageSuffix}";
                    return false;
                }
                if (digit < DigitMinVal || digit > DigitMaxVal)
                {
                    message = $"Each digit must be between {DigitMinVal} and {DigitMaxVal}{messageSuffix}";
                    return false;
                }
            }
            return true;
        }
    }
}
