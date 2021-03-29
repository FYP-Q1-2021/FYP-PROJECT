using System.Collections.Generic;
using UnityEngine;

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
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
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
}
