using UnityEngine;
using System;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private GameObject vfx;
    [SerializeField] private Transform spawnPosition;

    public event Action OnSpawnBoss;

    private bool isPassed;

    void OnTriggerEnter(Collider other)
    {
        if (!isPassed)
        {
            isPassed = true;
            objectToSpawn.SetActive(true);
            OnSpawnBoss?.Invoke();
            GameObject effects = Instantiate(vfx, spawnPosition.position, spawnPosition.rotation);
            Destroy(effects, 10f);
        }
    }
}
