using System.Collections;
using UnityEngine;

public class ExpandingRing : MonoBehaviour
{
    private Vector3 originalScale;
    [SerializeField] private Vector3 finalScale = new Vector3(100f, 5f, 100f);
    [SerializeField] private float scaleSpeed = 1f;
    [SerializeField] private float scaleDuration = 10f;
    private bool expanding;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered");
    }

    public void ExpandingRingAttack()
    {
        if (expanding)
            return;

        gameObject.SetActive(true);
        expanding = true;
        StartCoroutine("ExpandOverTime");
    }

    IEnumerator ExpandOverTime()
    {
        float currentTime = 0f;

        while(currentTime < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, finalScale, currentTime / scaleDuration);
            currentTime += scaleSpeed * Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        expanding = false;
        gameObject.SetActive(false);
    }
}
