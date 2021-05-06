using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Checks if scene has been loaded
/// </summary>
public class SceneLoadManager : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name));
    }

    void Start()
    {
        Transform spawnLocation = null;

        if (!PlayerSpawnManager.Instance.isReturning)
        {
            if (SceneManager.GetActiveScene().name == "Playtest" || SceneManager.GetActiveScene().name == "Playtest2")
            {
                spawnLocation = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>();
            }
            else if (SceneManager.GetActiveScene().name == "Playtest3")
            {
                spawnLocation = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>();
            }
            else
            {
                Debug.Log("Incorrect scene name");
            }
        }
        else
        {
            PlayerSpawnManager.Instance.isReturning = false;

            if (SceneManager.GetActiveScene().name == "Playtest" || SceneManager.GetActiveScene().name == "Playtest2")
            {
                spawnLocation = GameObject.Find("PlayerReturnPoint").GetComponent<Transform>();
            }
            else if (SceneManager.GetActiveScene().name == "Playtest3")
            {
                spawnLocation = GameObject.Find("PlayerReturnPoint").GetComponent<Transform>();
            }
            else
            {
                Debug.Log("Incorrect scene name");
            }
        }

        PlayerSpawnManager.Instance.player.GetComponent<CharacterController>().enabled = false;
        PlayerSpawnManager.Instance.player.gameObject.transform.position = spawnLocation.position;
        PlayerSpawnManager.Instance.player.gameObject.transform.rotation = Quaternion.Euler(spawnLocation.rotation.eulerAngles);
        PlayerSpawnManager.Instance.player.GetComponent<CharacterController>().enabled = true;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
