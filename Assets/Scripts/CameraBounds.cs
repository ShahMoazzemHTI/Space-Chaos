using UnityEngine;

public class CameraBounds : MonoBehaviour
{

    public static CameraBounds Instance { get; private set; }

    // Public read-only bounds
    public float MinX { get; private set; }
    public float MaxX { get; private set; }
    public float MinY { get; private set; }
    public float MaxY { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        CalculateBounds(); // Initialize values
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

    private void CalculateBounds()
    {
        float orthographicSize = Camera.main.orthographicSize;
        float horizontalExtent = orthographicSize * Camera.main.aspect;

        // float paddingX = 0.5f;
        // float paddingY = 0.5f;

        // MinX = -horizontalExtent + paddingX;
        // MaxX = horizontalExtent - paddingX;
        // MinY = -orthographicSize + paddingY;
        // MaxY = orthographicSize - paddingY;

        MinX = -horizontalExtent ;
        MaxX = horizontalExtent ;
        MinY = -orthographicSize ;
        MaxY = orthographicSize ;
    }
}

