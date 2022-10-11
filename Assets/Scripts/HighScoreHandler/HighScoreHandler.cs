using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    private string highScoreKey = "highScore";

    public void SetHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt(highScoreKey, 0))
        {
            PlayerPrefs.SetInt(highScoreKey, score);
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(highScoreKey,0);
    }
}
