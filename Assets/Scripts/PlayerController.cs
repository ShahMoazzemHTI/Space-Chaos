using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    [Header("settings")]
    [SerializeField] float moveSpeed;
    [Header("Boost Settings")] //mainly control via parallxBackground.cs class
    [SerializeField] float boostPower = 5f;
    [SerializeField] float maxEnergy;
    [SerializeField] float energyLossRate;
    [SerializeField] float energyRegenRate;
    [SerializeField] ParticleSystem engineBoostParticale;
    [SerializeField, Range(0f, 1f), Tooltip("Minimum energy (0-1) required to enable boost, e.g., 0.2 means 20% of maxEnergy")]
    float minEnergyPercentNormalized = 0.2f; // e.g., 0.2 = 20%

    //Health
    [Header("Health Setting")]
    [SerializeField] float maxHealth;
    [SerializeField] float hitAmount;
    [SerializeField] GameObject destroyBlastEffect;

    //hit and white flash
    [Header("Hit taking Setting")]
    Material defaultMaterial;
    [SerializeField] Material whiteFlashMaterial;
    [SerializeField] float invisibilityTime =1f;
    [SerializeField] float flashInterval = 0.2f;
    SpriteRenderer spriteRenderer;

    

    [Header("Caches")]
    float currentEnegry;
    float currentHealth;
    bool isBoosting;

    float minX, maxX, minY, maxY;

    //this value bellow is used by ParallaxBackground to create the boost effect;
    public float boost = 1f;




    Rigidbody2D rb;
    Vector2 moveDirection;
    Animator animator;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        currentEnegry = maxEnergy;
        currentHealth = maxHealth;
        UIController.Instance.UpdateEnergySlider(currentEnegry, maxEnergy);
        UIController.Instance.UpdateHealthSlider(currentHealth, maxHealth);
        // GetCameraBound();
        // Debug.Log($"min x --> {minX} \n max x --> {maxX} \n min y --> {minY} \n max y --> {maxY}");
        var bounds = CameraBounds.Instance.GetCameraBounds(); //⚠️⚠️⚠️⚠️⚠️CameraBounds class must be attaced to any gameObject.in this case it attached to the mainCamera
        minX = bounds.minX;
        maxX = bounds.maxX;
        minY = bounds.minY;
        maxY = bounds.maxY;

    }

    void Update()
    {
        if (Time.timeScale <= 0f) return; // <- Add this line to skip logic when paused

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
        {
            EnterBoostMode();
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))
        {
            ExitBoostMode();
        }

        if (isBoosting)
        {
            currentEnegry -= energyLossRate * Time.deltaTime;

            if (currentEnegry <= 0)
            {
                currentEnegry = 0f;
                ExitBoostMode();
            }
            UIController.Instance.UpdateEnergySlider(currentEnegry, maxEnergy);

        }
        else
        {
            currentEnegry += energyRegenRate * Time.deltaTime;
            if (currentEnegry >= maxEnergy) currentEnegry = maxEnergy;
            UIController.Instance.UpdateEnergySlider(currentEnegry, maxEnergy);
        }


        if (Input.GetButtonDown("Fire1"))
        {
            PhaserWeapon.Instance.shoot();
        }

        BoundPlayerPositionInsideScreen();


    }

    void FixedUpdate()
    {
        if (Time.timeScale <= 0f) return; // <- Add this line to skip logic when paused

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed * Time.fixedDeltaTime, moveDirection.y * moveSpeed * Time.fixedDeltaTime);
        // Debug.Log(rb.linearVelocity);
    }

    void EnterBoostMode()
    {
        float requiredEnergy = maxEnergy * minEnergyPercentNormalized;
        if (currentEnegry < requiredEnergy) return;

        boost = boostPower;
        isBoosting = true;
        engineBoostParticale.Play();
        AudioManager.Instance.PlaySound(AudioManager.Instance.fire);
        animator.SetBool("Boosting", true);
    }

    void ExitBoostMode()
    {
        boost = 1f;
        isBoosting = false;
        animator.SetBool("Boosting", false);
    }

    //collision

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentHealth -= hitAmount;
            UIController.Instance.UpdateHealthSlider(currentHealth, maxHealth);
            AudioManager.Instance.PlaySound(AudioManager.Instance.hit);
            StartCoroutine(WhiteFlash());
        }

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Instantiate(destroyBlastEffect, transform.position, transform.rotation);
            boost = 0f;
            GameManager.Instance.GameOver();
            AudioManager.Instance.PlaySound(AudioManager.Instance.ice);
        }
    }


    // void GetCameraBound()
    // {
    //     float orthographicSize = Camera.main.orthographicSize; // half screen height
    //     float horizontalExtent = orthographicSize * Camera.main.aspect; // half screen width

    //     float paddingX = 0.5f; // adjust based on sprite size
    //     float paddingY = 0.5f;

    //     minX = -horizontalExtent + paddingX;
    //     maxX = horizontalExtent - paddingX;
    //     minY = -orthographicSize + paddingY;
    //     maxY = orthographicSize - paddingY;

    // }

    void BoundPlayerPositionInsideScreen()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    IEnumerator WhiteFlash()
    {
        

        float elapsedTIme = 0f;
        bool isWhite = false;

        while (elapsedTIme < invisibilityTime)
        {
            GetComponent<Collider2D>().enabled = false;
            spriteRenderer.material = isWhite ? whiteFlashMaterial : defaultMaterial;
            isWhite = !isWhite;

            yield return new WaitForSeconds(flashInterval / 2f);
            elapsedTIme += flashInterval / 2f;

        }
        GetComponent<Collider2D>().enabled = true;
        spriteRenderer.material = defaultMaterial;
        
    }

    public void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            ExitBoostMode();
        }
    }


}



