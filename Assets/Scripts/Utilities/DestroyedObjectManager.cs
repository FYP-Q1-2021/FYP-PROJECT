using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyedObjectManager : MonoBehaviour
{
    public static DestroyedObjectManager Instance { get; private set; }

    public List<string> destroyedObjects = new List<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddToDestroyedObjectsList(GameObject go)
    {
        destroyedObjects.Add(go.name + " " + SceneManager.GetActiveScene().name);
    }

    public void DeleteDestroyedObjectsAfterReload()
    {
        int numOfDestroyedObjects = destroyedObjects.Count;

        if (numOfDestroyedObjects == 0)
            return;

        MeshDestroy[] destructibleMeshes = FindObjectsOfType<MeshDestroy>();
        int numOfDestructibleMeshes = destructibleMeshes.Length;

        for (int i = 0; i < numOfDestructibleMeshes; ++i)
        {
            for (int j = 0; j < numOfDestroyedObjects; ++j)
            {
                if (string.Compare(destructibleMeshes[i].name + " " + SceneManager.GetActiveScene().name, destroyedObjects[j]) == 0)
                {
                    Destroy(destructibleMeshes[i].gameObject);
                }
            }
        }
    }
}
