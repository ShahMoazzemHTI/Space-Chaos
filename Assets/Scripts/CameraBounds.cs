using UnityEngine;

public class CameraBounds : MonoBehaviour
{

    public static CameraBounds Instance;

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

    public struct Bounds
    {
        public float minX, maxX, minY, maxY;

        public Bounds(float minX, float maxX, float minY, float maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }
    }

    public Bounds GetCameraBounds()
    {
        float orthographicSize = Camera.main.orthographicSize; // half screen height
        float horizontalExtent = orthographicSize * Camera.main.aspect; // half screen width

        float paddingX = 0.5f; // adjust based on sprite size
        float paddingY = 0.5f;

        float minX = -horizontalExtent + paddingX;
        float maxX = horizontalExtent - paddingX;
        float minY = -orthographicSize + paddingY;
        float maxY = orthographicSize - paddingY;

        return new Bounds(minX, maxX, minY, maxY);
    }
}

