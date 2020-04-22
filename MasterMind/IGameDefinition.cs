namespace MasterMind
{
    public interface IGameDefinition
    {
        int AllowedGuesses { get; }
        int GuessLength { get; }
        int DigitMinVal { get; }
        int DigitMaxVal { get; }

        bool ValidateInputFormat(string guess, out string message);
    }
}
