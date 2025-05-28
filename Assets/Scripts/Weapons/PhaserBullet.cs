using UnityEngine;

public class PhaserBullet : MonoBehaviour
{



    void Update()
    {
        transform.position += new Vector3(PhaserWeapon.Instance.speed * Time.deltaTime, 0f);

        if (transform.position.x > CameraBounds.Instance.MaxX)
        {
            gameObject.SetActive(false);
        }
    }

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Citter") || collision.gameObject.CompareTag("Boss"))
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
