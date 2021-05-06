using UnityEngine;

public class GoblinAnimationEvents : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void StopAttackAnimation()
    {
        animator.SetInteger("State", (int)State.PATROL);
    }
}