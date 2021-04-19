using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string currentRoom = "";
    [SerializeField] string nextRoom = "";
    [SerializeField] SpawnPointData spawnPointData;
    [SerializeField] bool backToPreviousRoom;

    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentRoom));
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine("SwitchScene");
    }

    IEnumerator SwitchScene()
    {
        // Wait for other scene to finish loading
        // Calls Start() in the other scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextRoom, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        // For portals the brings player back to previous room
        if(backToPreviousRoom)
        {
            PlayerSpawnManager.Instance.player.GetComponent<CharacterController>().enabled = false;
            PlayerSpawnManager.Instance.player.gameObject.transform.position = spawnPointData.position;
            PlayerSpawnManager.Instance.player.gameObject.transform.rotation = Quaternion.Euler(spawnPointData.rotation);
            PlayerSpawnManager.Instance.player.GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            // Move the player to the spawn location of that scene
            for (int i = 0; i < PlayerSpawnManager.Instance.spawnPoints.Count; ++i)
            {
                if (nextRoom == PlayerSpawnManager.Instance.spawnPoints[i].name)
                {
                    PlayerSpawnManager.Instance.player.GetComponent<CharacterController>().enabled = false;
                    PlayerSpawnManager.Instance.player.gameObject.transform.position = PlayerSpawnManager.Instance.spawnPoints[i].position;
                    PlayerSpawnManager.Instance.player.gameObject.transform.rotation = Quaternion.Euler(PlayerSpawnManager.Instance.spawnPoints[i].rotation);
                    PlayerSpawnManager.Instance.player.GetComponent<CharacterController>().enabled = true;
                }
            }
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(currentRoom));
        Resources.UnloadUnusedAssets();
    }
}
