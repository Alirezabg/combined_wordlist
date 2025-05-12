using combined_wordlist.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CombinedWordlist.Server.Tests
{
    public class WordSourceTests
    {
        [Fact]
        public void LoadWords_Should_Return_5_Letter_Words()
        {
            // Arrange
            var wordSource = new WordSource();
            // Act
            var words = wordSource.LoadWords();
            // Assert
            Assert.All(words, word => Assert.Equal(5, word.Length));
        }

        [Fact]
        public void GetRandomWord_Should_Return_Valid_Word()
        {
            // Arrange
            var wordSource = new WordSource();
            // Act
            var randomWord = wordSource.GetRandomWord();
            // Assert
            Assert.Contains(randomWord, wordSource.LoadWords());
        }

        [Fact]
        public void TestLoadWords()
        {
            // Arrange
            var wordSource = new WordSource();
            // Act
            var words = wordSource.LoadWords();
            var words2 = wordSource.LoadWords();

            // Assert
            Assert.NotEmpty(words);
            Assert.Equal(words.Count, words2.Count);
            Assert.All(words, word => Assert.Equal(5, word.Length));
            Assert.Same(words, words2);
        }
        [Fact]
        public void TestValidWord()
        {
            // Arrange
            var wordSource = new WordSource();
            var words = wordSource.LoadWords();
            var validWord = words.First();
            var invalidWord = "notValid";
            // Act
            var isValid = wordSource.IsValidWord(validWord);
            var isInvalid = wordSource.IsValidWord(invalidWord);
            // Assert
            Assert.True(isValid);
            Assert.False(isInvalid);
        }
        [Fact]
        public void TestRandomWord()
        {
            // Arrange
            var wordSource = new WordSource();
            var words = wordSource.LoadWords();
            // Act
            var randomWord = wordSource.GetRandomWord();
            var randomWord2 = wordSource.GetRandomWord();
            // Assert
            Assert.Contains(randomWord, words);
            Assert.Equal(5, randomWord.Length);
            Assert.Contains(randomWord2, words);
            Assert.Equal(5, randomWord2.Length);
            Assert.NotEqual(randomWord, randomWord2);
        }
    }
}
