using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float missileSpeed = 25f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * missileSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        GameObject otherObject = collision.gameObject;

        if (otherObject.CompareTag("Enemy")) 
        {
            
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(1);
            }
            else
            {
               
                Debug.LogWarning("GameManager.instance is null in MissileController. Cannot add score.");
            }

          
            if (GameManager.instance != null && GameManager.instance.explosion != null)
            {
                
                GameObject explosionEffect = Instantiate(GameManager.instance.explosion, otherObject.transform.position, Quaternion.identity);
                Destroy(explosionEffect, 2f);
            }
            else
            {
                Debug.LogWarning("Explosion Prefab or GameManager instance is null. Cannot spawn explosion effect.");
            }

            Destroy(this.gameObject);
            if (otherObject != null)
            {
                Destroy(otherObject);
            }
        }
    }
}
