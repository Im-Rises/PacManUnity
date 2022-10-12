using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private const string HighScoreKey = "highScore";
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    private int _score;
    public static ScoreHandler instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        scoreText.SetText("Score: " + _score);
        highScoreText.SetText("High Score: " + PlayerPrefs.GetInt(HighScoreKey));
    }

    public void ResetScore()
    {
        _score = 0;
        scoreText.SetText("Score: " + _score);
    }

    public void AddScore(int score)
    {
        _score += score;
        scoreText.SetText("Score: " + _score);
        SetHighScore();
    }

    private void SetHighScore()
    {
        if (_score > PlayerPrefs.GetInt(HighScoreKey))
        {
            PlayerPrefs.SetInt(HighScoreKey, _score);
            highScoreText.SetText("High Score: " + _score);
        }
    }
}
