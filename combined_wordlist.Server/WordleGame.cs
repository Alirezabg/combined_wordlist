public class WordleGame
{
    public string WordToGuess { get; set; } = string.Empty;
    public int MaxAttempts { get; set; } = 6;
    public int Attempts { get; set; }
    public HashSet<string> ValidWords { get; set; } = new();

    public WordleGame() { }

    public WordleGame(IEnumerable<string> validWords)
    {
        ValidWords = new HashSet<string>(validWords);
        var random = new Random();
        WordToGuess = ValidWords.ElementAt(random.Next(ValidWords.Count));
        Attempts = 0;
    }

    public string CheckGuess(string guess)
    {
        if (guess.Length != WordToGuess.Length)
            return "Incorrect word length!";
        if (!ValidWords.Contains(guess))
            return "Invalid word!";


        Attempts++;

        if (guess == WordToGuess)
            return "Correct! You win!";

        return GetHint(guess);
    }

    private string GetHint(string guess)
    {
        bool[] used = new bool[WordToGuess.Length];
        char[] result = new char[guess.Length];
        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == WordToGuess[i])
            {
                result[i] = 'G';
                used[i] = true;
            }
        }
        for (int i = 0; i < guess.Length; i++)
        {
            if (result[i] == 'G') continue;
            if (WordToGuess.Contains(guess[i]) && !used[WordToGuess.IndexOf(guess[i])])
            {
                result[i] = 'Y';
                used[WordToGuess.IndexOf(guess[i])] = true;
            }
            else
            {
                result[i] = 'B';
            }
        }
        return new string(result);
    }

    public bool IsGameOver() => Attempts >= MaxAttempts;
}
