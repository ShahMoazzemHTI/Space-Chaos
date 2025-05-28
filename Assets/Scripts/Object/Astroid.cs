using UnityEngine;
using System.Collections;
public class Astroid : MonoBehaviour
{

    [SerializeField] Sprite[] sprites;
    [SerializeField] float invisibilityTime;
    [SerializeField] Material whiteFlashMaterial;
    [SerializeField] float flashInterval;

    [SerializeField] int live;
    [SerializeField] GameObject destroyEffect;

    Material defaultMaterial;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;


    void Awake()
    {
        defaultMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        GivePush();
        float randomScale = Random.Range(0.6f, 1f);
        transform.localScale = new Vector3(randomScale, randomScale);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage(10);
        }
    }

    private void TakeDamage(int damage)
    {
        live -= damage;
        if (live <= 0)
        {
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.boom2);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.hitRock);
        StartCoroutine(WhiteFlash());
    }

    IEnumerator WhiteFlash()
    {
        

        float elapsedTIme = 0f;
        bool isWhite = false;

        while (elapsedTIme < invisibilityTime)
        {
            // GetComponent<Collider2D>().enabled = false;
            spriteRenderer.material = isWhite ? whiteFlashMaterial : defaultMaterial;
            isWhite = !isWhite;

            yield return new WaitForSeconds(flashInterval / 2f);
            elapsedTIme += flashInterval / 2f;

        }
        // GetComponent<Collider2D>().enabled = true;
        spriteRenderer.material = defaultMaterial;
        
    }
}
