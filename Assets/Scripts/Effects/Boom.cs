using UnityEngine;

public class Boom : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    
}
