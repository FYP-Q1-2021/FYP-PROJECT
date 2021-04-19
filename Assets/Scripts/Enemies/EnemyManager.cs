using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [Header("Goblins")]
    [SerializeField] private List<GameObject> goblins = new List<GameObject>();
    [Header("Imps")]
    [SerializeField] private List<GameObject> imps = new List<GameObject>();

    [SerializeField] private int numOfEnemies;

    void Start()
    {
        Instance = this;

        goblins.Clear();
        imps.Clear();

        goblins.AddRange(GameObject.FindGameObjectsWithTag("Goblin"));
        imps.AddRange(GameObject.FindGameObjectsWithTag("Imp"));

        numOfEnemies = goblins.Count + imps.Count;

        DeleteDeadEnemiesAfterReload();
    }

    void Update()
    {
        if (numOfEnemies == 0 && SceneManager.GetActiveScene().name == "TestingScene")
            GameEndingManager.Instance.winEnding = true;
    }

    public void RemoveFromList(GameObject go)
    {
        if (go.CompareTag("Goblin"))
            goblins.Remove(go);
        else if (go.CompareTag("Imp"))
            imps.Remove(go);

        --numOfEnemies;
    }

    // Used when player loses
    public void StopAllEnemies()
    {
        for (int i = 0; i < goblins.Count; ++i)
            goblins[i].GetComponent<NavMeshAgent>().isStopped = true;

        for (int i = 0; i < imps.Count; ++i)
            imps[i].GetComponent<Imp>().movementSpeed = 0f;
    }

    // Check through list of enemies in this scene for dead enemies and delete them
    private void DeleteDeadEnemiesAfterReload()
    {
        List<string> listOfDeadEnemies = DeadEnemyManager.Instance.deadEnemies;

        int numOfDeadEnemies = listOfDeadEnemies.Count;

        if (numOfDeadEnemies == 0)
            return;

        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(goblins);
        enemies.AddRange(imps);

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
