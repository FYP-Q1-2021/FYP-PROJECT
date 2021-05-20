using UnityEngine;
using System;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    public event Action OnSpawnBoss;

    private bool isPassed;

    void OnTriggerEnter(Collider other)
    {
        if (!isPassed)
        {
            isPassed = true;
            objectToSpawn.SetActive(true);
            OnSpawnBoss?.Invoke();
        }
    }
}
