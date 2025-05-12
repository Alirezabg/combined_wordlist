using combined_wordlist.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace combined_wordlist.Server.Services
{
    public class WordleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly WordSource _wordSource;


        public WordleService(IHttpContextAccessor httpContextAccessor, IMemoryCache cache, WordSource wordSource)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _wordSource = wordSource;
        }

        public WordleGame GetGame()
        {
            var context = _httpContextAccessor.HttpContext!;
            context.Session.SetString("init", "1"); // Stabilize SessionId

            var sessionId = context.Session.Id;
            var cacheKey = $"game-{sessionId}";

            if (!_cache.TryGetValue(cacheKey, out WordleGame game))
            {
                game = new WordleGame(_wordSource);
                _cache.Set(cacheKey, game);
            }

            return game;
        }

        public void ResetGame()
        {
            var context = _httpContextAccessor.HttpContext!;
            var sessionId = context.Session.Id;
            var cacheKey = $"game-{sessionId}";

            var newGame = new WordleGame(_wordSource);
            _cache.Set(cacheKey, newGame);
        }
    }
}
