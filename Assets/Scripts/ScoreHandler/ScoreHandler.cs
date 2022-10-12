using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private const string HighScoreKey = "highScore";
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public static ScoreHandler instance { get; private set; }

    private void Start()
    {
        scoreText.SetText("Score: 0");
        highScoreText.SetText("High Score: " + PlayerPrefs.GetInt(HighScoreKey));
    }

    public void UpdateHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt(HighScoreKey, 0)) PlayerPrefs.SetInt(HighScoreKey, score);
        highScoreText.SetText("High Score: " + PlayerPrefs.GetInt(HighScoreKey));
    }
}
