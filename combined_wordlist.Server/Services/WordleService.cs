using combined_wordlist.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;



namespace combined_wordlist.Server.Services
{
    public class WordleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private static List<string>? _wordsCache;

        public WordleService(IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
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
        public WordleGame GetGame(HttpContext context)
        {
            context.Session.SetString("init", "1");

            var sessionId = context.Session.Id;
            var cacheKey = $"game-{sessionId}";

            if (!_cache.TryGetValue(cacheKey, out WordleGame game))
            {
                game = new WordleGame(LoadWords());
                _cache.Set(cacheKey, game);
            }
            return game;
        }
        public void ResetGame()
        {
            var context = _httpContextAccessor.HttpContext!;
            var sessionId = context.Session.Id;
            var cacheKey = $"game-{sessionId}";
            var newGame = new WordleGame(LoadWords());
            _cache.Set(cacheKey, newGame);
        }


    }
}
