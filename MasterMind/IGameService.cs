namespace MasterMind
{
    public interface IGameService
    {
        GuessResult EvaluateGuess(string guess);
    }
}
