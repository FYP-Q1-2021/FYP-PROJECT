using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{

    public float minFlickerRange = 0.5f;
    public float maxFlickerRange = 1f;

    public int minBurst = 1;
    public int maxBurst = 5;

    [SerializeField]
    int burst;
    bool isBurst;
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
        // offs the light
        // get a random float and waits for it to turn on the light again
        // gets a new random float and allows for next set of flicker


        isFlickering = true;
        burst = Random.Range(minBurst, maxBurst);
        delayBetweenFlicker = Random.Range(minFlickerRange, maxFlickerRange);

        //this.gameObject.GetComponent<Light>().enabled = false;
        //yield return new WaitForSeconds(delayBetweenFlicker);
        //this.gameObject.GetComponent<Light>().enabled = true;

        while (burst > 1)
        {
            this.gameObject.GetComponent<Light>().enabled = false;
            yield return new WaitForSeconds(delayBetweenFlicker);
            this.gameObject.GetComponent<Light>().enabled = true;
            yield return new WaitForSeconds(delayBetweenFlicker);
            burst--;
        }

        delayBetweenFlicker = Random.Range(minFlickerRange, maxFlickerRange);
        yield return new WaitForSeconds(delayBetweenFlicker);
        isFlickering = false;
    }
}
