using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{

    public float shortFlicker = 0.5f;
    public float longFlicker = 1f;
    [SerializeField]
    float delayBetweenFlicker;
    bool isFlickering;

    // Update is called once per frame
    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(Flicker());
        }
    }

    IEnumerator Flicker()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        delayBetweenFlicker = Random.Range(shortFlicker, longFlicker);
        yield return new WaitForSeconds(delayBetweenFlicker);
        this.gameObject.GetComponent<Light>().enabled = true;
        delayBetweenFlicker = Random.Range(shortFlicker, longFlicker);
        yield return new WaitForSeconds(delayBetweenFlicker);
        isFlickering = false;
    }
}
