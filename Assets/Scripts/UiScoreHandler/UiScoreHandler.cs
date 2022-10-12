using TMPro;
using UnityEngine;

public class UiScoreHandler : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    private HighScoreHandler _highScoreHandler;

    private void Start()
    {
        scoreText.SetText("Score: 0");
        highScoreText.SetText("High Score: " + PlayerPrefs.GetInt("HighScore"));
    }

    public void UpdateHighScore(int score)
    {
        highScoreText.SetText("High Score: " + score);
    }
}
