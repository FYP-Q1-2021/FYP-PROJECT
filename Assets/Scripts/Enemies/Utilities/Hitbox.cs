using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    Health health;
    [SerializeField]
    float multiplier = 1f;

    void Awake()
    {
        health = GetComponent<Health>();
        if (!health)
        {
            health = GetComponentInParent<Health>();
        }
    }

    public void InflictDamage(float damage)
    {
        if(health)
        {
            health.Damage(damage * multiplier);
        }
    }
}
