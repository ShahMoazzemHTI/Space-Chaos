using UnityEngine;

public class Astroid : MonoBehaviour
{

    [SerializeField] Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        GivePush();
    }
    void Update()
    {
        MoveWithWorld();
    }

    void GivePush()
    {
        float pushX = Random.Range(-1f, 0);
        float pushY = Random.Range(-1f, 1f);

        rb.linearVelocity = new Vector2(pushX, pushY);
    }
    void MoveWithWorld()
    {
        transform.position += new Vector3((GameManager.Instance.worldSpeed * PlayerController.Instance.boost * Time.deltaTime), 0f);
    }
}
