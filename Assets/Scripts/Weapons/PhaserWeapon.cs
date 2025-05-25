using Unity.VisualScripting;
using UnityEngine;

public class PhaserWeapon : MonoBehaviour
{
    public static PhaserWeapon Instance;
    // [SerializeField] private GameObject prefab;
    [SerializeField] ObjectPool phaserBulletPool;

    public float speed;
    public float damage;

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

    public void shoot()
    {
        // Instantiate(prefab, transform.position, transform.rotation);
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.shoot);
        GameObject phaserBullet = phaserBulletPool.GetPooledObject();
        phaserBullet.transform.position = transform.position;
        phaserBullet.SetActive(true);
        

    }
}
