using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyedGameObjectsManager : MonoBehaviour
{
    public static DestroyedGameObjectsManager Instance { get; private set; }
    public List<string> destroyedGOList = new List<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddToDestroyedGOList(GameObject go)
    {
        destroyedGOList.Add(go.name + " " + SceneManager.GetActiveScene().name);
    }

    public void DeleteDestroyedGOAfterReload(System.Type type)
    {
        int numOfDestroyedGO = destroyedGOList.Count;

        if (numOfDestroyedGO == 0)
            return;

        Object[] go = FindObjectsOfType(type);

        int totalNumOfGO = go.Length;

        for (int i = 0; i < totalNumOfGO; ++i)
        {
            for (int j = 0; j < numOfDestroyedGO; ++j)
            {
                if (string.Compare(go[i].name + " " + SceneManager.GetActiveScene().name, destroyedGOList[j]) == 0)
                {
                    // Just destroys the component
                    Destroy(go[i]);
                }
            }
        }
    }
}
