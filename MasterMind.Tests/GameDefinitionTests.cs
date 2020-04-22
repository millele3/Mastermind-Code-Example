using System;
using Xunit;
using Xunit.Abstractions;

namespace MasterMind.Tests
{
    public class GameDefinitionTests
    {
        private readonly ITestOutputHelper output;

        public GameDefinitionTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // Run some quick basic tests on the input format checker
        [Theory]
        [InlineData(null, 1, 1, 1, false)]
        [InlineData("", 1, 1, 1, false)]
        [InlineData("X", 1, 1, 1, false)]
        [InlineData("123x", 4, 1, 3, false)]
        [InlineData("1234", 4, 1, 3, false)]
        [InlineData("123", 4, 1, 3, false)]
        [InlineData("12 3", 4, 1, 3, false)]
        [InlineData("1234", 4, 1, 4, true)]
        public void ValidateInputFormat_DataDriven(string guess, int guessLength, int digitMinVal, int digitMaxVal, bool expectedResult)
        {
            output.WriteLine($"Testing: guess:{guess}, guessLength:{guessLength}, digitMinVal:{digitMinVal}, digitMaxVal:{digitMaxVal}, expectedResult:{expectedResult}");
            IGameDefinition definition = new GameDefinition(1, guessLength, digitMinVal, digitMaxVal);
            var result = definition.ValidateInputFormat(guess, out string message);

            // If success we expect no message, if fail, we expect a message
            var messageAsExpected = result ? (string.IsNullOrEmpty(message)) : (message.Length > 0);

            Assert.Equal(expectedResult, result);
            Assert.True(messageAsExpected, "Message output not as expected");
        }
    }
}
