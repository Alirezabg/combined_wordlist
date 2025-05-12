using combined_wordlist.Server.Models;
using combined_wordlist.Server.Services;
using Moq;
using Xunit;

namespace CombinedWordlist.Server.Tests
{
    public class WordleGameTests
    {

        [Fact]
        public void CheckGuess_Returns_Correct_Answer()
        {
            // Arrange
            var words = new Mock<WordSource>();
            words.Setup(w => w.GetRandomWord()).Returns("apple");
            words.Setup(w => w.IsValidWord("apple")).Returns(true);
            var game = new WordleGame(words.Object);
            // Act
            var result = game.CheckGuess("apple");
            // Assert
            Assert.Equal("GGGGG", result);
        }

        [Theory]
        [InlineData("apple", "apple", "GGGGG")]
        [InlineData("apple", "aaple", "GBGGG")]
        [InlineData("apple", "aaale", "GBBGG")]
        public void CheckGuess_Returns_Hint_For_Partial_Match(string wordToGuess, string guess, string expected)
        {
            // Arrange
            var words = new Mock<WordSource>();
            words.Setup(w => w.GetRandomWord()).Returns(wordToGuess);
            words.Setup(w => w.IsValidWord(guess)).Returns(true);
            var game = new WordleGame(words.Object);

            // Act
            var result = game.CheckGuess(guess);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CheckGuess_Invalid_Word_Returns_Invalid()
        {
            // Arrange
            var words = new Mock<WordSource>();
            words.Setup(w => w.GetRandomWord()).Returns("apple");
            words.Setup(w => w.IsValidWord("zzzzz")).Returns(false);
            var game = new WordleGame(words.Object);


            // Act
            var result = game.CheckGuess("zzzzz");

            // Assert
            Assert.Equal("Invalid word!", result);
        }

        [Fact]
        public void CheckGuess_Wrong_Length_Returns_Error()
        {
            // Arrange
            var words = new Mock<WordSource>();
            words.Setup(w => w.GetRandomWord()).Returns("apple");
            words.Setup(w => w.IsValidWord("app")).Returns(false);
            var game = new WordleGame(words.Object);

            // Act
            var result = game.CheckGuess("app");

            // Assert
            Assert.Equal("Incorrect word length!", result);
        }
    }
}
