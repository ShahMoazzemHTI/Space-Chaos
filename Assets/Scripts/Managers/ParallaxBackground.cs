using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    float backgroundImageWidth;

    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        backgroundImageWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void Update()
    {
        float moveX = (moveSpeed * GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;
        transform.position += new Vector3(moveX, transform.position.y);
        if (Mathf.Abs(transform.position.x) - backgroundImageWidth > 0)
        {
            transform.position = Vector3.zero;
        }

        
    }
}
