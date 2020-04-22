using Xunit;
using Xunit.Abstractions;
using NSubstitute;

namespace MasterMind.Tests
{
    public class GameServiceTests
    {
        private readonly ITestOutputHelper output;
        
        public GameServiceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private IGameDefinition GetDameDefinition(int allowedGuesses = 2, int guessLength = 4, int digitMinVal = 1, int digitMaxVal = 6)
        {
            var definition = Substitute.For<IGameDefinition>();
            definition.AllowedGuesses.Returns(allowedGuesses);
            definition.GuessLength.Returns(guessLength);
            definition.DigitMinVal.Returns(digitMinVal);
            definition.DigitMaxVal.Returns(digitMaxVal);
            definition.ValidateInputFormat(Arg.Any<string>(), out Arg.Any<string>()).Returns(true);
            return definition;
        }

        // Run some quick basic tests on the game service
        [Fact]
        public void EvaluateGuess_CorrectGuess()
        {
            var gameService = new GameService();
            gameService.NewGame(GetDameDefinition(), "1234");
            var result = gameService.EvaluateGuess("1234");
            Assert.Equal(ResultCodes.CorrectGuess, result.Result);
            Assert.Contains("[++++]", result.Message);
        }
        [Fact]
        public void EvaluateGuess_InCorrectTryAgainPattern1()
        {
            var gameService = new GameService();
            gameService.NewGame(GetDameDefinition(), "1234");
            var result = gameService.EvaluateGuess("1235");
            Assert.Equal(ResultCodes.InCorrectTryAgain, result.Result);
            Assert.Contains("[+++ ]", result.Message);
        }
        [Fact]
        public void EvaluateGuess_InCorrectTryAgainPattern2()
        {
            var gameService = new GameService();
            gameService.NewGame(GetDameDefinition(), "1234");
            var result = gameService.EvaluateGuess("4321");
            Assert.Equal(ResultCodes.InCorrectTryAgain, result.Result);
            Assert.Contains("[----]", result.Message);
        }
        [Fact]
        public void EvaluateGuess_InCorrectTryAgainPattern3()
        {
            var gameService = new GameService();
            gameService.NewGame(GetDameDefinition(), "1234");
            var result = gameService.EvaluateGuess("5656");
            Assert.Equal(ResultCodes.InCorrectTryAgain, result.Result);
            Assert.Contains("[    ]", result.Message);
        }
        [Fact]
        public void EvaluateGuess_InCorrectTryAgainPattern4()
        {
            var gameService = new GameService();
            gameService.NewGame(GetDameDefinition(), "1234");
            var result = gameService.EvaluateGuess("1134");
            Assert.Equal(ResultCodes.InCorrectTryAgain, result.Result);
            Assert.Contains("[+-++]", result.Message);
        }
        [Fact]
        public void EvaluateGuess_NoMoreTries()
        {
            var gameService = new GameService();
            gameService.NewGame(GetDameDefinition(), "1234");
            var result = gameService.EvaluateGuess("1134");
            result = gameService.EvaluateGuess("1244");
            Assert.Equal(ResultCodes.NoMoreTries, result.Result);
            Assert.Contains("[++-+]", result.Message);
        }

        [Fact]
        public void EvaluateGuess_EnsureExceptionWhenSpecifyBadCode()
        {
            var gameService = new GameService();

            var definition = Substitute.For<IGameDefinition>();
            definition.ValidateInputFormat(Arg.Any<string>(), out Arg.Any<string>()).Returns(false);

            var thrown = false;
            try
            {
                gameService.NewGame(definition, "X");
            }
            catch
            {
                thrown = true;
            }
            Assert.True(thrown);
        }
    }
}
