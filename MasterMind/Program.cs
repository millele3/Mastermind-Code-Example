using System;

namespace MasterMind
{
    class Program
    {
        // These could be in appSettings or specified on command-line to make it flexible
        public const int AllowedGuesses = 10;
        public const int GuessLength = 4;
        public const int DigitMinVal = 1;
        public const int DigitMaxVal = 6;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            const string exitCode = "X";
            IGameDefinition gameDefinition = new GameDefinition(AllowedGuesses, GuessLength, DigitMinVal, DigitMaxVal);
            var gameService = new GameService();
            gameService.NewGame(gameDefinition);

            Console.WriteLine($"Enter {GuessLength}-digit guess(each digit between {DigitMinVal} and {DigitMaxVal}), you have {AllowedGuesses} tries. '{exitCode}' to exit:");

            while(1==1)
            {
                var guess = Console.ReadLine().ToLower();
                if (guess == exitCode.ToLower()) break;
                Console.WriteLine(gameService.EvaluateGuess(guess).Message);
            }
        }
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
