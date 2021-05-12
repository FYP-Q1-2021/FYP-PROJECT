using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    private bool isPassed;

    void OnTriggerEnter(Collider other)
    {
        if (!isPassed)
        {
            isPassed = true;
            if(objectToSpawn != null)
                objectToSpawn.SetActive(true);
        }
    }
}
