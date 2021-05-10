using UnityEngine;
using System;

public class GoblinAnimationEvents : MonoBehaviour
{
    private Animator animator;
    public event Action OnLastAttackFrame;

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void StopAttackAnimation()
    {
        OnLastAttackFrame?.Invoke();
        animator.SetInteger("State", (int)State.PATROL);
    }
}