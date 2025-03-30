using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

[ApiController]
[Route("api/wordle")]
public class WordleController : ControllerBase
{
    private const string SessionKey = "WordleGame";
    public WordleController()
    {
    }
    private static List<string> LoadWords()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "words.txt");

        return System.IO.File.ReadAllLines(filePath).
            Where(word => word.Length ==5).
            Select(word => word.ToLower() ).
            Distinct().
            ToList();

    }
    private WordleGame GetGame()
    {
        var gameData = HttpContext.Session.GetString(SessionKey);
        if (gameData != null)
        {
            return JsonSerializer.Deserialize<WordleGame>(gameData)!;
        }

        var newGame = new WordleGame( LoadWords());
        SaveGame(newGame);
        return newGame;
    }

    private void SaveGame(WordleGame game)
    {
        var gameData = JsonSerializer.Serialize(game);
        HttpContext.Session.SetString(SessionKey, gameData);
    }

    [HttpPost("guess")]
    public IActionResult MakeGuess([FromBody] string guess)
    {
        var game = GetGame();
        var response = game.CheckGuess(guess);
        SaveGame(game);
        return Ok(new { response, gameOver = game.IsGameOver() });
    }

    [HttpGet("reset")]
    public IActionResult ResetGame()
    {
        var newGame = new WordleGame(LoadWords());
        SaveGame(newGame);
        HttpContext.Session.Remove("RevealedLetters");

        return Ok("Game reset successfully!");
    }

    [HttpGet("help")]
    public IActionResult RevealLetter()
    {
        var game = GetGame();
        var word = game.WordToGuess;
        const string revealedKey = "RevealedLetters";

        // Load revealed indexes from session
        List<int> revealedIndexes = new();
        var revealedData = HttpContext.Session.GetString(revealedKey);
        if (!string.IsNullOrEmpty(revealedData))
        {
            revealedIndexes = JsonSerializer.Deserialize<List<int>>(revealedData)!;
        }

        // Get unrevealed positions
        var unrevealed = Enumerable.Range(0, word.Length)
            .Where(i => !revealedIndexes.Contains(i))
            .ToList();

        if (unrevealed.Count == 0)
        {
            char invalidResult = ' ';
            return Ok(new { position = 0, invalidResult });
        }

        // Pick random unrevealed letter
        var random = new Random();
        int index = unrevealed[random.Next(unrevealed.Count)];
        char letter = word[index];

        // Save revealed index
        revealedIndexes.Add(index);
        HttpContext.Session.SetString(revealedKey, JsonSerializer.Serialize(revealedIndexes));

        return Ok(new { position = index, letter });
    }

    [HttpGet("solve")]
    public IActionResult SolvePuzzle()
    {
        var originalGame = GetGame();

        // Clone the game with same word, but fresh attempt count
        var solverGame = new WordleGame(originalGame.ValidWords)
        {
            WordToGuess = originalGame.WordToGuess
        };

        var possibleWords = new List<string>(solverGame.ValidWords);
        var attempts = new List<object>();

        while (!solverGame.IsGameOver())
        {
            var guess = possibleWords.First(); // simple strategy: try first
            var hint = solverGame.CheckGuess(guess);

            attempts.Add(new { guess, hint });

            if (hint == "Correct! You win!")
                break;

            // Filter possible words based on hint
            possibleWords = possibleWords
                .Where(word => MatchesHint(word, guess, hint))
                .ToList();
        }

        return Ok(new
        {
            word = solverGame.WordToGuess,
            totalAttempts = solverGame.Attempts,
            steps = attempts
        });
    }
    private bool MatchesHint(string word, string guess, string hint)
    {
        for (int i = 0; i < guess.Length; i++)
        {
            if (hint[i] == 'G' && word[i] != guess[i]) return false;
            if (hint[i] == 'Y' && (word[i] == guess[i] || !word.Contains(guess[i]))) return false;
            if (hint[i] == 'B' && word.Contains(guess[i])) return false;
        }
        return true;
    }

}
