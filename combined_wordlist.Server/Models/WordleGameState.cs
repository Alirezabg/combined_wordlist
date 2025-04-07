namespace combined_wordlist.Server.Models
{
    public class WordleGameState
    {
        public string WordToGuess { get; set; } = string.Empty;
        public int? Attempts { get; set; }
    }
}
