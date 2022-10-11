using TMPro;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    private HighScoreHandler _highScoreHandler;

    private void Start()
    {
        highScoreText.SetText("High Score: " + PlayerPrefs.GetInt("HighScore"));
    }
}
