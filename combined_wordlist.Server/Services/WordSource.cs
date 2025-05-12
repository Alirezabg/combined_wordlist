namespace combined_wordlist.Server.Services
{
    public class WordSource
    {
        private readonly Lazy<List<string>> _wordsCache = new(() =>
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "words.txt");
            return System.IO.File.ReadAllLines(filePath)
                .Where(word => word.Length == 5)
                .Select(word => word.ToLower())
                .Distinct()
                .ToList();
        });
        public virtual List<string> LoadWords()
        {
            return _wordsCache.Value;
        }
        public virtual void ClearCache()
        {
            _wordsCache.Value.Clear();
        }
        public virtual string GetRandomWord()
        {
            var words = _wordsCache.Value;
            var random = new Random();
            int index = random.Next(words.Count);
            return words[index];
        }
        public virtual bool IsValidWord(string word)
        {
            return _wordsCache.Value.Contains(word.ToLower());
        }
    }
}
