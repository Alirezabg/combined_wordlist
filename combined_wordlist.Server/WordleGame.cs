using combined_wordlist.Server.Data.Enum;
using combined_wordlist.Server.Services;

public class GuessResult
{
    public required StatusCode Status { get; init; }
    public required LetterClue Clue { get; init; }
}
public class WordleGame
{
    public string WordToGuess { get; set; } = string.Empty;
    public int MaxAttempts { get; set; } = 6;
    public int Attempts { get; set; }
    private readonly WordSource _wordSource;

    public WordleGame() { }

    public WordleGame(WordSource wordSource)
    {
        _wordSource = wordSource;
        WordToGuess = _wordSource.GetRandomWord();
        Attempts = 0;
    }

    public string CheckGuess(string guess)
    {
        if (guess.Length != WordToGuess.Length)
            return "Incorrect word length!";
        if (!_wordSource.IsValidWord(guess))
            return "Invalid word!";


        Attempts++;

        if (guess == WordToGuess)
            return "GGGGG";

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
