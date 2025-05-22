using UnityEngine;
using UnityEngine.SceneManagement;

public class LostWhale : MonoBehaviour
{
    void Update()
    {
        MoveWithWorld();
    }

    void MoveWithWorld()
    {
        transform.position += new Vector3((GameManager.Instance.worldSpeed * PlayerController.Instance.boost * Time.deltaTime), 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level_1_Complete");
        }
    }
}
