using System.Collections;
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
    private LayerMask explosionLayer;
    private bool isExplodeCoroutineRunning;

    [Header("Debug Display")]
    public Color explosionRangeColor = Color.red;

    private Health playerHP;

    #region Unity Callbacks
    void Start()
    {
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        explosionLayer = LayerMask.GetMask("Player");
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
                if(!isExplodeCoroutineRunning)
                    StartCoroutine("Explode");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
            playerHP.Damage(projectileDamage);

        ResetValues();
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = explosionRangeColor;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    #endregion

    #region Public functions
    public void SetDirection(Vector3 dir)
    {
        attackDir = dir;
        // Rotate crystal
    }
    #endregion

    #region Coroutines
    IEnumerator Explode()
    {
        isExplodeCoroutineRunning = true;

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayer);

        for (int i = 0; i < nearbyObjects.Length; ++i)
        {
            if(nearbyObjects[i].name == "Player")
            {
                playerHP.Damage(explosionDamage);
            }
        }

        // Particle effect
        yield return new WaitForSeconds(0.1f);
        ResetValues();
        gameObject.SetActive(false);
    }
    #endregion

    #region Public functions
    private void Move()
    {
        transform.position += attackDir * movementSpeed * Time.deltaTime;
    }

    private void ResetValues()
    {
        travelingTimeCounter = 0f;
        explosionCounter = 0f;
        isExplodeCoroutineRunning = false;
    }
    #endregion
}
