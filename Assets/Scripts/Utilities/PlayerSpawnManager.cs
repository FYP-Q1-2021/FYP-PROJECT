using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }

    [SerializeField] public Transform player;

    public bool isReturning;
    public string prevSceneName;

    public List<SpawnPointData> spawnPoints = new List<SpawnPointData>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
