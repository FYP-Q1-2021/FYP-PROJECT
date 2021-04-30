using UnityEngine;

public class MultiDirectionalBillboard : MonoBehaviour
{
    private Transform player;
    private Animator animator;

    private readonly string parameter = "SideSeen";

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
        
        if(dot > -0.5f && dot < 0.5f)
        {
            // Right
            if (dirOfPlayerFromEnemy < 0f)
                animator.SetFloat(parameter, 0.4285714f);
            // Left
            else
                animator.SetFloat(parameter, 0.1428571f);
        }
        else if(dot > -0.75f && dot < -0.5f)
        {
            // Front left
            if(dirOfPlayerFromEnemy > 0f)
                animator.SetFloat(parameter, 0.7142857f);
            // Front right
            else
                animator.SetFloat(parameter, 0.57142857f);
        }
        else if(dot > 0.5f && dot < 0.75f)
        {
            // Back left
            if (dirOfPlayerFromEnemy > 0f)
                animator.SetFloat(parameter, 1f);
            // Back right
            else
                animator.SetFloat(parameter, 0.8571429f);
        }
        else if (dot <= -0.5f)
        {
            // Front
            animator.SetFloat(parameter, 0f);
        }
        else if (dot >= 0.5f)
        {
            // Back
            animator.SetFloat(parameter, 0.2857143f);
        }
    }
}
