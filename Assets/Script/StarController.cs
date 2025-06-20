using UnityEngine;

public class StarController : MonoBehaviour
{
    public float rotationSpeed = 0f;
    public AudioManager audioManager;
    [Header("Scaling Effect")]
    public float scaleSpeed = 1f;
    public float scaleRange = 0.2f;
    private Vector3 initialScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = transform.localScale;
    }
    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        float scaleOffset = Mathf.PingPong(Time.time * scaleSpeed, scaleRange);
        float scaleValue = 1f - (scaleRange / 2f) + scaleOffset;
        transform.localScale = initialScale * scaleValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                audioManager.PlaySFX(audioManager.coinClip);
                GameManager.instance.AddScore(10);
            }
            else
            {
                Debug.LogWarning("GameManager instance is null. Cannot add score for star collection.");
            }

            // Destroy the collectible
            Destroy(gameObject);
        }
    }
}
