using combined_wordlist.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace CombinedWordlist.Server.Tests
{
    public class FakeWordleService : WordleService
    {
        public FakeWordleService(IHttpContextAccessor accessor, IMemoryCache cache)
            : base(accessor, cache)
        {
        }

        protected override List<string> LoadWords()
        {
            return new List<string> { "apple", "grape", "berry" };
        }
    }
}
