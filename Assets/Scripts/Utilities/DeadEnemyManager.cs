using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadEnemyManager : MonoBehaviour
{
    public static DeadEnemyManager Instance { get; private set; }

    public List<string> deadEnemies = new List<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddToDeadEnemiesList(GameObject go)
    {
        deadEnemies.Add(go.name + " " + SceneManager.GetActiveScene().name);
    }
}
