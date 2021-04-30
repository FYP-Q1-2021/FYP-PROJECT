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
    [Header("Pirates")]
    [SerializeField] private List<GameObject> pirates = new List<GameObject>();
    [Header("Devil")]
    [SerializeField] private GameObject devil;

    [SerializeField] private int numOfEnemies;

    void Start()
    {
        Instance = this;

        goblins.Clear();
        imps.Clear();
        pirates.Clear();

        goblins.AddRange(GameObject.FindGameObjectsWithTag("Goblin"));
        imps.AddRange(GameObject.FindGameObjectsWithTag("Imp"));
        pirates.AddRange(GameObject.FindGameObjectsWithTag("Pirate"));
        devil = GameObject.FindGameObjectWithTag("Devil");

        numOfEnemies = goblins.Count + imps.Count + pirates.Count;

        if (devil)
            ++numOfEnemies;

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
        else if (go.CompareTag("Pirate"))
            pirates.Remove(go);

        --numOfEnemies;
    }

    // Used when player loses
    public void StopAllEnemies()
    {
        for (int i = 0; i < goblins.Count; ++i)
            goblins[i].GetComponent<NavMeshAgent>().isStopped = true;

        for (int i = 0; i < imps.Count; ++i)
            imps[i].GetComponent<Imp>().movementSpeed = 0f;

        for (int i = 0; i < pirates.Count; ++i)
            pirates[i].GetComponent<NavMeshAgent>().isStopped = true;
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
        enemies.AddRange(pirates);
        enemies.Add(devil);

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
