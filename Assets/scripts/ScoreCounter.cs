using UnityEngine;
using TMPro; 

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    private int score = 0;

    private void Start()
    {
        UpdateScoreText();
    }

    public void IncreaseScore()
    {
        score++;
        UpdateScoreText();
    }

    public void CheckHighScore()
    {
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
