using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [Header("Goblins")]
    [SerializeField] private List<GameObject> goblins = new List<GameObject>();
    [Header("Imps")]
    [SerializeField] private List<GameObject> imps = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        //else
            //Destroy(gameObject);
    }

    void Start()
    {
        goblins.AddRange(GameObject.FindGameObjectsWithTag("Goblin"));
        imps.AddRange(GameObject.FindGameObjectsWithTag("Imp"));
    }

    public void RemoveFromList(GameObject go)
    {
        if (go.CompareTag("Goblin"))
            goblins.Remove(go);
        else if (go.CompareTag("Imp"))
            imps.Remove(go);
    }

    // Used when player loses
    public void StopAllEnemies()
    {
        for(int i = 0; i < goblins.Count; ++i)
            goblins[i].GetComponent<NavMeshAgent>().isStopped = true;

        for(int i = 0; i < imps.Count; ++i)
            imps[i].GetComponent<Imp>().movementSpeed = 0f;
    }
}
