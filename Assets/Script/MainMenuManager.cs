using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public AudioManager audioManager;
    void Start()
    {
        DisplayFinalScore();
    }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found! Make sure it is tagged 'Audio'.");
        }
    }
    public void PlayGame()
    {
        audioManager.PlaySFX(audioManager.clickClip);
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
