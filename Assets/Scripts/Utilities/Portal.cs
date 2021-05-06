using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string currentRoom = "";
    [SerializeField] string nextRoom = "";
    [SerializeField] SpawnPointData spawnPointData;
    [SerializeField] bool backToPreviousRoom;
    private bool isTriggered;

    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (backToPreviousRoom)
                PlayerSpawnManager.Instance.isReturning = true;
            StartCoroutine("SwitchScene");
            isTriggered = true;
        }
    }

    IEnumerator SwitchScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(currentRoom));
        Resources.UnloadUnusedAssets();
        SceneManager.LoadSceneAsync(nextRoom, LoadSceneMode.Additive);
        yield return null;
    }
}
