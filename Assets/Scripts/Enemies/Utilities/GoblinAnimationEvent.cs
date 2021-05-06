using UnityEngine;

public class GoblinAnimationEvent : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void StopAttackAnimation()
    {
        animator.SetInteger("State", (int)State.PATROL);
    }
}
