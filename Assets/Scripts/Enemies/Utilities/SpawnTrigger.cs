using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    void OnTriggerEnter(Collider other)
    {
        objectToSpawn.SetActive(true);
        Destroy(gameObject);
    }
}
