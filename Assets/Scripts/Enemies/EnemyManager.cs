using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// This will always be executed first
/// - see Script Execution Order
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    public int numOfEnemies;

    public event Action OnEnemyDeath;

    void Start()
    {
        Instance = this;

        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        numOfEnemies = enemies.Count;

        DeleteDeadEnemiesAfterReload();
    }

    public void RemoveFromList(GameObject go)
    {
        enemies.Remove(go);
        --numOfEnemies;

        DeadEnemyManager.Instance.AddToDeadEnemiesList(go);
        OnEnemyDeath?.Invoke();
    }

    // Used when player loses
    public void StopAllEnemies()
    {
        for(int i = 0; i < numOfEnemies; ++i)
        {
            NavMeshAgent agent = enemies[i].GetComponent<NavMeshAgent>();
            Imp imp = enemies[i].GetComponent<Imp>();
            if (agent)
                agent.isStopped = true;
            else if(imp)
                imp.movementSpeed = 0f;
        }
    }

    // Check through list of enemies in this scene for dead enemies and delete them
    private void DeleteDeadEnemiesAfterReload()
    {
        List<string> listOfDeadEnemies = DeadEnemyManager.Instance.deadEnemies;

        int numOfDeadEnemies = listOfDeadEnemies.Count;

        if (numOfDeadEnemies == 0)
            return;

        for (int i = 0; i < numOfEnemies; ++i)
        {
            for (int j = 0; j < numOfDeadEnemies; ++j)
            {
                if (string.Compare(enemies[i].name + " " + SceneManager.GetActiveScene().name, listOfDeadEnemies[j]) == 0)
                    Destroy(enemies[i]);
            }
        }
    }
}
