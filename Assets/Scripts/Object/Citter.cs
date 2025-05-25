using UnityEngine;

public class Citter : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;

    float elapsedTime = 0f;
    float intervalTime;
    float moveSpeed = 1f;

    Vector3 targetPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        moveSpeed = Random.Range(0.5f, 3f);
        intervalTime = Random.Range(0.1f, 2f);
        elapsedTime = intervalTime;

        GenerateRandomPosition();
    }

    void Update()
    {
        // Handle interval timer
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
        }
        else
        {
            GenerateRandomPosition();
            intervalTime = Random.Range(0.1f, 2f);
            elapsedTime = intervalTime;
        }

        // Movement and rotation
        Vector3 direction = targetPosition - transform.position;

        if (direction.magnitude > 0.01f)
        {
            // Move toward target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Use LookRotation and rotate only on Z-axis
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction); // Z-axis forward
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 1080 * Time.deltaTime);
        }

        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;
        transform.position += new Vector3(moveX, 0);
    }

    private void GenerateRandomPosition()
    {
        float randomX = Random.Range(-9f, 9f);
        float randomY = Random.Range(-5f, 5f);
        targetPosition = new Vector3(randomX, randomY, 0);
    }
}
