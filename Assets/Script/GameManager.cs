using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject enemyPrefab;
    public float minInstantiateValue;
    public float maxInstantiateValue;
    public float destroyTime;

    public static GameManager instance;
    [Header("Particle Effects")]
    public GameObject explosion;
    public GameObject muzzleFlash;

    [Header("Scoring")]
    public int currentScore = 0; 
    public TextMeshProUGUI scoreTextUI;


    [Header("Star Spawning")]
    public GameObject starPrefab; 
    public float starSpawnRate = 3f; 
    public float starDestroyTime = 10f;
    private Camera mainCamera;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Make sure your main camera is tagged 'MainCamera'.");
        }
    }
    private void Start()
    {
        InvokeRepeating("InstantiateEnemy",1f,2f);
        currentScore = 0;
        UpdateScoreUI();
        if (starPrefab == null)
        {
            Debug.LogError("DEBUG: Star Prefab is NULL in GameManager.Start()! Please assign it in the Inspector.");
            return;
        }
        else
        {
            Debug.Log("DEBUG: Star Prefab is assigned in GameManager.Start(): " + starPrefab.name);
            InvokeRepeating("InstantiateStar", 3f, starSpawnRate);

        }
    }

    // Update is called once per frame
    void InstantiateEnemy()
    {
        Vector3 enemypos = new Vector3(Random.Range(minInstantiateValue, maxInstantiateValue), 6f);
        GameObject enemy =  Instantiate(enemyPrefab, enemypos, Quaternion.Euler(0f,0f,180f));
        Destroy(enemy,destroyTime);
    }
    public void InstantiateStar()
    {
        if (starPrefab == null)
        {
            Debug.LogError("RUNTIME ERROR: Star Prefab is not assigned in GameManager! Cannot instantiate star. (Likely GameManager.instance was destroyed)"); //
            CancelInvoke("InstantiateStar");
            return;
        }
        if (mainCamera == null)
        {
            Debug.LogError("RUNTIME ERROR: Main Camera is null! Cannot instantiate star.");
            CancelInvoke("InstantiateStar"); 
            return;
        }
        float cameraHalfHeight = mainCamera.orthographicSize;
        float cameraHalfWidth = mainCamera.aspect * cameraHalfHeight;

        float minX = -cameraHalfWidth + 1f; 
        float maxX = cameraHalfWidth - 1f;
        float minY = -cameraHalfHeight + 1f;
        float maxY = cameraHalfHeight - 1f;
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 starPos = new Vector3(randomX, randomY, 0f);
        GameObject star = Instantiate(starPrefab, starPos, Quaternion.identity); 
        Destroy(star, starDestroyTime);
    }
    public void AddScore(int addNumber)
    {
        currentScore += addNumber;
        UpdateScoreUI();
    }
    void UpdateScoreUI()
    {
        scoreTextUI.text  = currentScore.ToString();    
    }
    public int GetFinalScore()
    {
        return currentScore;
    }
}
 