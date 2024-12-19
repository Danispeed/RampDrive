using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public void LoadGame()
    {
        SceneManager.LoadScene("Game"); 
    }

    private void Start()
    {
        scoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
