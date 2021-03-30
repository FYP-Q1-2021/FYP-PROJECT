using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private int damage = 10;

    void OnTriggerEnter(Collider other)
    {
        health.Damage(damage);
    }
}
