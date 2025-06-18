using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed;
    public float rotationSpeed = 45f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
