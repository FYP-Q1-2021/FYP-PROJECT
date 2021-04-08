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

    void OnTriggerEnter(Collider col)
    {
        WeaponController weapcontroller = col.gameObject.GetComponent<WeaponController>();
        if(weapcontroller != null)
        {
            health.Damage(weapcontroller.damage);
        }
        ProjectileStandard projectStandard = col.gameObject.GetComponent<ProjectileStandard>();
        if (projectStandard != null)
        {
            health.Damage(projectStandard.damage);
        }
    }
}
