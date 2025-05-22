using UnityEngine;

public class Whale : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    void Update()
    {
        MoveWithWorld();
    }

    void MoveWithWorld()
    {
        transform.position += new Vector3((GameManager.Instance.worldSpeed * PlayerController.Instance.boost * Time.deltaTime), 0f);
    }
}
