using UnityEngine;

public class Boom : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }


    void Update()
    {
        MoveWithWorld();
    }
    void MoveWithWorld()
    {
        transform.position += new Vector3((GameManager.Instance.worldSpeed * PlayerController.Instance.boost * Time.deltaTime), 0f);
    }

    
}
