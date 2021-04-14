using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string currentRoom = "";
    [SerializeField] string nextRoom = "";

    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentRoom));
    }

    // TODO: Have to set player and weapon in another layer then make it so that portal layer only collides with player layer
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine("SwitchScene");
    }

    IEnumerator SwitchScene()
    {
        // Wait for other scene to finish loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextRoom, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

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

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(currentRoom));
        Resources.UnloadUnusedAssets();
    }
}
