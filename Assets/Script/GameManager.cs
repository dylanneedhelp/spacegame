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
        InvokeRepeating("InstantiateEnemy", 1f, 2f);
        currentScore = 0;
        if (starPrefab != null)
            InvokeRepeating(nameof(InstantiateStar), 3f, starSpawnRate);
        else
            Debug.LogError("Star Prefab not assigned in GameManager.Awake()");

        UpdateScoreUI();
    }

    // Update is called once per frame
    void InstantiateEnemy()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        float camHeight = mainCamera.orthographicSize * 2f;
        float camWidth = camHeight * mainCamera.aspect;

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        int meteorCount = 2;

        for (int i = 0; i < meteorCount; i++)
        {
            Vector2 spawnDir = directions[Random.Range(0, directions.Length)];

            Vector2 spawnPos2D = Vector2.zero;
            Vector2 camCenter = mainCamera.transform.position;

            float padding = 1.5f;

            if (spawnDir == Vector2.up || spawnDir == Vector2.down)
            {
                float xOffset = Random.Range(-camWidth / 2f, camWidth / 2f);
                float yPos = camCenter.y + (spawnDir.y > 0 ? (camHeight / 2f + padding) : -(camHeight / 2f + padding));
                spawnPos2D = new Vector2(camCenter.x + xOffset, yPos);
            }
            else if (spawnDir == Vector2.left || spawnDir == Vector2.right)
            {
                float yOffset = Random.Range(-camHeight / 2f, camHeight / 2f);
                float xPos = camCenter.x + (spawnDir.x > 0 ? (camWidth / 2f + padding) : -(camWidth / 2f + padding));
                spawnPos2D = new Vector2(xPos, camCenter.y + yOffset);
            }

            Vector3 spawnPos = new Vector3(spawnPos2D.x, spawnPos2D.y, 0f);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 moveDir = (camCenter - spawnPos2D).normalized;
                rb.linearVelocity = moveDir * Random.Range(0.5f, 1.2f);
            }

            Destroy(enemy, destroyTime);
        }

    }
    public void InstantiateStar()
    {
        if (starPrefab == null)
        {
            Debug.LogError("RUNTIME ERROR: Star Prefab is not assigned in GameManager! Cannot instantiate star. (Likely GameManager.instance was destroyed)");
            CancelInvoke("InstantiateStar");
            return;
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("RUNTIME ERROR: Main Camera is null! Cannot instantiate star.");
                CancelInvoke("InstantiateStar");
                return;
            }
        }
        Debug.Log("InstantiateStar called"); // For debugging
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
        scoreTextUI.text = currentScore.ToString();
    }
    public int GetFinalScore()
    {
        return currentScore;
    }
}

