using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Position setting")]
    [SerializeField] float topPadding;
    [SerializeField] float bottomPadding;
    [SerializeField] float leftPadding;
    float maxY, minY, spawnX;

    public static ObjectSpawner Instance { get; private set; }

    public float MaxY => maxY;
    public float MinY => minY;
    public float SpawnX => spawnX;

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
        var bounds = CameraBounds.Instance.GetCameraBounds();
        spawnX = bounds.maxX + leftPadding;
        maxY = bounds.maxY - topPadding;
        minY = bounds.minY - bottomPadding;
    }


}
