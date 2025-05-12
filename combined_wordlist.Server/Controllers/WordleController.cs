using combined_wordlist.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
[ApiController]
[Route("api/wordle")]
public class WordleController : ControllerBase
{
    private const string SessionKey = "WordleGame";
    private readonly WordleService _wordleService;

    public WordleController(WordleService wordleService)
    {
        _wordleService = wordleService;
    }

    [HttpPost("guess")]
    public IActionResult MakeGuess([FromBody] string guess)
    {
        var game = _wordleService.GetGame();
        var response = game.CheckGuess(guess);
        return Ok(new { response, gameOver = game.IsGameOver() });
    }

    [HttpGet("reset")]
    public IActionResult ResetGame()
    {
        _wordleService.ResetGame();
        HttpContext.Session.Remove("RevealedLetters");
        return Ok("Game reset successfully!");
    }

    [HttpGet("help")]
    public IActionResult RevealLetter()
    {
        var game = _wordleService.GetGame();
        var word = game.WordToGuess;
        const string revealedKey = "RevealedLetters";

        List<int> revealedIndexes = new();
        var revealedData = HttpContext.Session.GetString(revealedKey);
        if (!string.IsNullOrEmpty(revealedData))
        {
            revealedIndexes = JsonSerializer.Deserialize<List<int>>(revealedData)!;
        }

        var unrevealed = Enumerable.Range(0, word.Length)
            .Where(i => !revealedIndexes.Contains(i))
            .ToList();

        if (unrevealed.Count == 0)
        {
            return Ok(new { message = "All letters have already been revealed!" });
        }

        var random = new Random();
        int index = random.Next(unrevealed.Count);
        char letter = word[index];

        revealedIndexes.Add(index);
        HttpContext.Session.SetString(revealedKey, JsonSerializer.Serialize(revealedIndexes));

        return Ok(new { position = index, letter });
    }

    [HttpGet("solve")]
    public IActionResult SolvePuzzle()
    {
        var originalGame = _wordleService.GetGame();

        var solverGame = new WordleGame(originalGame.ValidWords)
        {
            WordToGuess = originalGame.WordToGuess
        };

        var possibleWords = new List<string>(solverGame.ValidWords);
        var attempts = new List<object>();

        while (!solverGame.IsGameOver())
        {
            var guess = possibleWords.First();
            var hint = solverGame.CheckGuess(guess);

            attempts.Add(new { guess, hint });

            if (hint == "Correct! You win!")
                break;

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
