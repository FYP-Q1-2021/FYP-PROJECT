using UnityEngine;

public class MultiDirectionalBillboard : MonoBehaviour
{
    private Transform player;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float dot = Vector3.Dot(player.forward, transform.forward);

        Vector3 dir = (player.position - transform.position).normalized;
        float dirOfPlayerFromEnemy = Vector3.Dot(transform.right, dir);

        if (dot > -0.5f && dot < 0.5f && dirOfPlayerFromEnemy < 0f)
        {
            animator.SetFloat("SideSeen", 1f);
            //animator.SetInteger("SideSeen", (int)Direction.Right);
        }
        else if (dot > -0.5f && dot < 0.5f && dirOfPlayerFromEnemy > 0f)
        {
            animator.SetFloat("SideSeen", 0.33f);
            //animator.SetInteger("SideSeen", (int)Direction.Left);
        }
        else if (dot <= -0.5f)
        {
            animator.SetFloat("SideSeen", 0f);
            //animator.SetInteger("SideSeen", (int)Direction.Front);
        }
        else if (dot >= 0.5f)
        {
            animator.SetFloat("SideSeen", 0.66f);
            //animator.SetInteger("SideSeen", (int)Direction.Back);
        }
    }
}
