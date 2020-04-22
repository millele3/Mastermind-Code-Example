namespace MasterMind
{
    public enum ResultCodes
    {
        InvalidInput,
        CorrectGuess,
        NoMoreTries,
        InCorrectTryAgain
    }

    public class GuessResult
    {
        public ResultCodes Result { get; set; }
        public string Message {get; set;}
    }
}
