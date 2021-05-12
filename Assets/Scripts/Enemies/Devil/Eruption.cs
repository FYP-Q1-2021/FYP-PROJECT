using System.Collections;
using UnityEngine;

public class Eruption : MonoBehaviour
{
    [SerializeField] private float eruptionTime = 2f;
    [SerializeField] private float eruptionDuration = 5f;
    private BoxCollider boxCollider;
    private float timer = 0f;

    private Health playerHP;
    [SerializeField] private float damage = 10f;

    [SerializeField] private GameObject vfx;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void OnEnable()
    {
        StartCoroutine("Erupt");

        GameObject effects = Instantiate(vfx, transform.parent.position, vfx.transform.rotation, transform.parent.transform);
        Destroy(effects, 6f);
    }

    void OnDisable()
    {
        boxCollider.enabled = false;
        timer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
            playerHP.Damage(damage);
    }

    IEnumerator Erupt()
    {
        while (timer < eruptionTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        boxCollider.enabled = true;
        timer = 0f;

        StartCoroutine("Erupting");
    }

    IEnumerator Erupting()
    {
        while (timer < eruptionDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);
    }
}
