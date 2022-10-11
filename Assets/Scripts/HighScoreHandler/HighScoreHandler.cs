using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    private const string HighScoreKey = "highScore";

    public void SetHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt(HighScoreKey, 0)) PlayerPrefs.SetInt(HighScoreKey, score);
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }
}
