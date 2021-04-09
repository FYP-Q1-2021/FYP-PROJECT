using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    Health health;

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
            health.Damage(damage);
        }
    }
}
