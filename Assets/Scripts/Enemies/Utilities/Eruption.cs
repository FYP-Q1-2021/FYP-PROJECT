using System.Collections;
using UnityEngine;

public class Eruption : MonoBehaviour
{
    [SerializeField] private float eruptionTime = 2f;
    [SerializeField] private float eruptionDuration = 5f;
    [SerializeField] private new ParticleSystem particleSystem;
    private BoxCollider boxCollider;
    private float timer = 0f;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        particleSystem.Stop();

        StartCoroutine("Erupt");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered");
    }

    IEnumerator Erupt()
    {
        while(timer < eruptionTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        boxCollider.enabled = true;
        timer = 0f;
        particleSystem.Play();

        StartCoroutine("Erupting");
    }

    IEnumerator Erupting()
    {
        while (timer < eruptionDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(transform.parent.gameObject);
    }
}
