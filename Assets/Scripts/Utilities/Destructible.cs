using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion;

    void OnTriggerEnter(Collider other)
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
