using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string currentRoom = "";
    [SerializeField] string nextRoom = "";

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine("SwitchScene");
    }

    IEnumerator SwitchScene()
    {
        SceneManager.LoadScene(nextRoom, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(currentRoom), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        Resources.UnloadUnusedAssets();

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

        yield return null;
        //yield return new WaitForSeconds(1f);
    }
}
