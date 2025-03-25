using combined_wordlist.Server.Controllers;
using combined_wordlist.Server.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/wordle")]
public class WordleController : ControllerBase
{
    private const string SessionKey = "WordleGame";
    private readonly WordleDbContext _context;
    public WordleController(WordleDbContext context)
    {
        _context = context;
    }

    private WordleGame GetGame()
    {
        var gameData = HttpContext.Session.GetString(SessionKey);
        if (gameData != null)
        {
            return JsonSerializer.Deserialize<WordleGame>(gameData)!;
        }

        var newGame = new WordleGame(new List<string> { "apple", "table", "chair", "bread", "grape" });
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
        var newGame = new WordleGame(new List<string> { "apple", "table", "chair", "bread", "grape" });
        SaveGame(newGame);
        return Ok("Game reset successfully!");
    }
}
