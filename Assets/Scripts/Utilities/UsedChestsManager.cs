using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UsedChestsManager : MonoBehaviour
{
    public static UsedChestsManager Instance { get; private set; }

    public List<string> usedChestsList = new List<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddToUsedChestsList(GameObject go)
    {
        usedChestsList.Add(go.name + " " + SceneManager.GetActiveScene().name);
    }

    public void DeleteUsedChestsAfterReload()
    {
        int numOfUsedChests = usedChestsList.Count;

        if (numOfUsedChests == 0)
            return;

        ChestSpawnManager[] chests = FindObjectsOfType<ChestSpawnManager>();
        int numOfDestructibleMeshes = chests.Length;

        for (int i = 0; i < numOfDestructibleMeshes; ++i)
        {
            for (int j = 0; j < numOfUsedChests; ++j)
            {
                if (string.Compare(chests[i].name + " " + SceneManager.GetActiveScene().name, usedChestsList[j]) == 0)
                {
                    Destroy(chests[i].gameObject);
                }
            }
        }
    }
}
