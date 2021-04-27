using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float projectileDamage = 10f;
    private Vector3 attackDir;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float maximumTravelTime = 10f;
    private float travelingTimeCounter = 0f;

    [Header("Explosion")]
    [SerializeField] private float explosionDamage = 20f;
    [SerializeField] private float explosionRadius = 30f;
    [SerializeField] private float timeUntilExplosion = 10f;
    private float explosionCounter = 0f;

    private Health playerHP;

    void Start()
    {
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        if (travelingTimeCounter < maximumTravelTime)
        {
            travelingTimeCounter += Time.deltaTime;
            Move();
        }
        // Crystal is not moving anymore
        else
        {
            if (explosionCounter < timeUntilExplosion)
            {
                explosionCounter += Time.deltaTime;
            }
            else
            {
                StartCoroutine("Explode");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
            playerHP.Damage(projectileDamage);
    }

    public void SetDirection(Vector3 dir)
    {
        attackDir = dir;
        // Rotate crystal
    }

    private void Move()
    {
        transform.position += attackDir * movementSpeed * Time.deltaTime;
    }

    IEnumerator Explode()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = 0; i < nearbyObjects.Length; ++i)
        {
            if(nearbyObjects[i].name == "Player")
            {

            }
        }

        // Particle effect
        yield return new WaitForSeconds(1f);
        ResetValues();
        gameObject.SetActive(false);
    }

    private void ResetValues()
    {
        travelingTimeCounter = 0f;
        explosionCounter = 0f;
    }
}
