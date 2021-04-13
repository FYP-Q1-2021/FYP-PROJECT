using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string nextRoom = "";

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine("LoadNextScene");
    }

    IEnumerator LoadNextScene()
    {
        SceneManager.LoadScene(nextRoom, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        for (int i = 0; i < PlayerSpawnManager.Instance.spawnPoints.Count; ++i)
        {
            if (nextRoom == PlayerSpawnManager.Instance.spawnPoints[i].name)
            {
                PlayerSpawnManager.Instance.player.SetPositionAndRotation(PlayerSpawnManager.Instance.spawnPoints[i].position, Quaternion.Euler(PlayerSpawnManager.Instance.spawnPoints[i].rotation));
                PlayerSpawnManager.Instance.MovePlayerToThisScene(nextRoom);
            }
        }

        yield return new WaitForSeconds(1f);
    }
}
