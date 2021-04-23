using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private Vector3 attackDir;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float maximumTravelTime = 10f;
    private float travelingTimeCounter = 0f;
    [SerializeField] private float timeUntilExplosion = 10f;
    private float explosionCounter = 0f;


    void Update()
    {
        if(travelingTimeCounter < maximumTravelTime)
        {
            travelingTimeCounter += Time.deltaTime;
            Move();
        }
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
        // Particle effect
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
