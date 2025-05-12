using combined_wordlist.Server.Models;
using Xunit;

namespace CombinedWordlist.Server.Tests
{
    public class WordleGameTests
    {
        [Fact]
        public void TestLoadWords()
        {
            // Arrange
            var words = new WordSource();
            var words = wordSource.LoadWords();

        }

        [Fact]
        public void CheckGuess_Returns_Hint_For_Partial_Match()
        {
            // Arrange
            var words = new List<string> { "apple" ,"allee" };
            var game = new WordleGame(words) { WordToGuess = "apple" };

            // Act
            var result = game.CheckGuess("allee");

            // Assert
            Assert.Equal("GYBBG", result);
        }

        [Fact]
        public void CheckGuess_Invalid_Word_Returns_Invalid()
        {
            // Arrange
            var words = new List<string> { "apple" };
            var game = new WordleGame(words) { WordToGuess = "apple" };

            // Act
            var result = game.CheckGuess("zzzzz");

            // Assert
            Assert.Equal("Invalid word!", result);
        }

        [Fact]
        public void CheckGuess_Wrong_Length_Returns_Error()
        {
            // Arrange
            var words = new List<string> { "apple" };
            var game = new WordleGame(words) { WordToGuess = "apple" };

            // Act
            var result = game.CheckGuess("app");

            // Assert
            Assert.Equal("Incorrect word length!", result);
        }
    }
}
