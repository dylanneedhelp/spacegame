using UnityEngine;
using UnityEngine.SceneManagement;

public class StarController : MonoBehaviour
{
    public float rotationSpeed=0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
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
