using combined_wordlist.Server.Models;
using System.Text.Json;



namespace combined_wordlist.Server.Services
{
    public class WordleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string Sesstionkey ="wordleGame";

        public WordleService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public List<string> LoadWords()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "words.txt");
            return System.IO.File.ReadAllLines(filePath)
                .Where(word => word.Length == 5)
                .Select(word => word.ToLower())
                .Distinct()
                .ToList();
        }
        public WordleGame GetGame()
        {
            var gameData = _httpContextAccessor.HttpContext.Session.GetString(Sesstionkey);
            if (gameData != null)
            {
                var state = JsonSerializer.Deserialize<WordleGameState>(gameData)!;
                var game = new WordleGame(LoadWords())
                {
                    WordToGuess = state.WordToGuess,
                    Attempts = state.Attempts ?? 0
                };
                return game;
            }
            var newGame = new WordleGame(LoadWords());
            SaveGame(newGame);
            return newGame;
        }

        public void SaveGame(WordleGame game)
        {
            var state = new WordleGameState
            {
                WordToGuess = game.WordToGuess,
                Attempts = game.Attempts
            };
            var gameData = JsonSerializer.Serialize(state);
            _httpContextAccessor.HttpContext.Session.SetString(Sesstionkey, gameData);
        }

    }
}
