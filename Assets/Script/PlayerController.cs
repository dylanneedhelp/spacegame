using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public AudioManager audioManager; // Reference to the AudioManager for playing sounds
    // Public variables
    public float speed = 5f; // The speed at which the player moves
    public bool canMoveDiagonally = true; // Controls whether the player can move diagonally
    public float rotationOffset = -90f;
    // Private variables 
    private Rigidbody2D rb; // Reference to the Rigidbody2D component attached to the player
    private Vector2 movement; // Stores the direction of player movement
    private bool isMovingHorizontally = true; // Flag to track if the player is moving horizontally
    [Header("Missile")]
    public GameObject missile;
    public Transform missileSpawnPosition;
    public float destroyTime = 5f;
    public Transform muzzleSpawnPosition;
  
    void Start()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Prevent the player from rotating
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void Awake()
    {
        // Find the AudioManager in the scene
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found! Make sure it is tagged 'Audio'.");
        }   
    }

    void Update()
    {
        // Get player input from keyboard or controller
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Check if diagonal movement is allowed
        if (canMoveDiagonally)
        {
            // Set movement direction based on input
            movement = new Vector2(horizontalInput, verticalInput);
            // Optionally rotate the player based on movement direction
            RotatePlayer(horizontalInput, verticalInput);
        }
        else
        {
            // Determine the priority of movement based on input
            if (horizontalInput != 0)
            {
                isMovingHorizontally = true;
            }
            else if (verticalInput != 0)
            {
                isMovingHorizontally = false;
            }
            else if (horizontalInput == 0 && verticalInput == 0)
            {
                
            }

            // Set movement direction and optionally rotate the player
            if (isMovingHorizontally)
            {
                movement = new Vector2(horizontalInput, 0);
                RotatePlayer(horizontalInput, 0);
            }
            else
            {
                movement = new Vector2(0, verticalInput);
                RotatePlayer(0, verticalInput);
            }
        }
        PlayerShoot();
    }

    void FixedUpdate()
    {
        // Apply movement to the player in FixedUpdate for physics consistency
        rb.linearVelocity = movement * speed;
    }

    void RotatePlayer(float x, float y)
    {
        // If there is no input, do not rotate the player   
        if (x == 0 && y == 0) return;

        // Calculate the rotation angle based on input direction
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg; angle += rotationOffset;
        // Apply the rotation to the player
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void PlayerShoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioManager.PlaySFX(audioManager.missileClip);
            SpawnMuzzle();
            SpawnMissle();
        }
    }
    void SpawnMissle()
    {
        GameObject gm = Instantiate(missile, missileSpawnPosition);
        
        gm.transform.SetParent(null);

        Destroy(gm, destroyTime);
    }
    void SpawnMuzzle()
    {
        GameObject muzzle = Instantiate(GameManager.instance.muzzleFlash, muzzleSpawnPosition);
        muzzle.transform.SetParent(null);
        Destroy(muzzle, destroyTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            audioManager.PlaySFX(audioManager.DeadClip);
            Destroy(this.gameObject);

            
            PlayerPrefs.SetInt("LastScore", GameManager.instance.GetFinalScore());
            //GameOver Scene
            Debug.Log("Game Over! Final Score: " + PlayerPrefs.GetInt("LastScore", 0));
            SceneManager.LoadScene("DeadScene");
        }



    }

}