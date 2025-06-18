using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    void Start()
    {
        DisplayFinalScore();
    }
    public void PlayGame()
    {
        Debug.Log("Play button clicked! Loading Gameplay Scene...");
        PlayerPrefs.SetInt("LastScore", 0);
        SceneManager.LoadScene("SampleScene");
        
    }

    public void DisplayFinalScore()
    {
        if (finalScoreText != null)
        {
            
            int score = PlayerPrefs.GetInt("LastScore", 0); 
            finalScoreText.text = "Your Score: " + score.ToString();
        }
        else
        {
            
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game button clicked!");
        Application.Quit();
#if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }
}
