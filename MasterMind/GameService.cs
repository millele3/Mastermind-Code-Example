using System;

namespace MasterMind
{   
    public class GameService : IGameService
    {
        private string _code;
        private IGameDefinition _definition;
        private int _remainingGuesses;

        public void NewGame(IGameDefinition definition, string code = null)
        {
            _definition = definition;
            _remainingGuesses = _definition.AllowedGuesses;

            // Allow the caller to specify the code if they wish
            _code = string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                if (!_definition.ValidateInputFormat(code, out string message))
                {
                    throw new Exception($"Specified code passed is not consistent with game definition:{message}");
                }
                _code = code;
            }
            else
            {
                // Generate a random code
                for (var digit = 0; digit < _definition.GuessLength; digit++)
                {
                    Random random = new Random();
                    _code += random.Next(_definition.DigitMinVal, _definition.DigitMaxVal).ToString();
                }
            }
        }

        public GuessResult EvaluateGuess(string guess)
        {
            // Ensure we have valid input
            if(!_definition.ValidateInputFormat(guess, out string result))
            {
                return new GuessResult { Result = ResultCodes.InvalidInput, Message = result };
            }

            // Ok they entered valid input, now decrement remaining guesses and check their guess vs code
            _remainingGuesses--;
            var responseCode = new char[_definition.GuessLength];
            var anyIncorrect = false;

            // Kinda brute-force
            for (var digit = 0; digit < _definition.GuessLength; digit++)
            {
                responseCode[digit] = ' ';
                if (_code.Contains(guess[digit]))
                {
                    responseCode[digit] = '-';
                }
                if (_code[digit] == guess[digit])
                {
                    responseCode[digit] = '+';
                }
                if (responseCode[digit] == ' ' || responseCode[digit] == '-')
                {
                    anyIncorrect = true;
                }
            }
            var correctGuess = !anyIncorrect;
            string responseString = new string(responseCode);

            // Evaluate results
            if (correctGuess)
            {
                var tries = _definition.AllowedGuesses - _remainingGuesses;
                NewGame(_definition);
                return new GuessResult
                {
                    Result = ResultCodes.CorrectGuess,
                    Message = $"[{responseString}] - Correct guess in {tries} tries! New Game!"
                };
            }
            else if (_remainingGuesses == 0)
            {
                NewGame(_definition);
                return new GuessResult
                {
                    Result = ResultCodes.NoMoreTries,
                    Message = $"[{responseString}] - Sorry, no more tries. The code was:{_code}, New Game!"
                };
            }

            return new GuessResult
            {
                Result = ResultCodes.InCorrectTryAgain,
                Message = $"[{responseString}] - {_remainingGuesses} more tries."
            };
        }
    }
}
