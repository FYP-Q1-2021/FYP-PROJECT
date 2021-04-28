using UnityEngine;

public class RippleCollider : MonoBehaviour
{
    private Health playerHP;
    private float damage;
    private int playerLayer;

    void Start()
    {
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        damage = GetComponentInParent<Ripple>().damage;
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
            playerHP.Damage(damage);

        /*        if (other.gameObject.layer == playerLayer)
                {
                    playerHP.Damage(damage);
                }*/
    }
}