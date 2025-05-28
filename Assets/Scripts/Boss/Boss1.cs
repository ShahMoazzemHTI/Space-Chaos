using UnityEngine;

public class Boss1 : MonoBehaviour
{
    float speedX;
    float speedY;
    bool charging;

    float switchInterval;
    float switchTimer;
    int live;
    Animator animator;


    void Awake()
    {
        live = 100;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        EnterChargeState();
    }

    void Update()
    {
        if (switchTimer > 0)
        {
            switchTimer -= Time.deltaTime;
        }
        else
        {
            if (charging)
            {
                EnterPatrolState();
            }
            else
            {
                EnterChargeState();
            }
        }

        if (transform.position.y > 3 || transform.position.y < -3)
        {
            speedY *= -1;
        }
        float moveX = speedX * PlayerController.Instance.boost * Time.deltaTime;
        float moveY = speedY * Time.deltaTime;

        transform.position += new Vector3(-moveX, moveY);
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }

    void EnterPatrolState()
    {
        animator.SetBool("isCharging", true);
        speedX = 0;
        speedY = Random.Range(-2f, 2f);
        switchInterval = Random.Range(5f, 10f);
        switchTimer = switchInterval;
        charging = false;
    }

    void EnterChargeState()
    {
        animator.SetBool("isCharging", true);
        speedX = 5f;
        speedY = 0;
        switchInterval = switchInterval = Random.Range(1f, 2.5f); ;
        switchTimer = switchInterval;
        charging = true;
        AudioManager.Instance.PlaySound(AudioManager.Instance.bossCharge);
    }

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.armorHit);
        live -= damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(0);
        }
    }
}
