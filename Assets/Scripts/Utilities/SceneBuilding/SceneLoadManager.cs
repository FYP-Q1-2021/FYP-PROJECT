﻿using UnityEngine;
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
            spawnLocation = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>();
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
                if(PlayerSpawnManager.Instance.prevSceneName == "Playtest4")
                    spawnLocation = GameObject.Find("PlayerReturnPoint4").GetComponent<Transform>();
                else if (PlayerSpawnManager.Instance.prevSceneName == "Playtest5")
                    spawnLocation = GameObject.Find("PlayerReturnPoint5").GetComponent<Transform>();
                else if (PlayerSpawnManager.Instance.prevSceneName == "PlayTest6")
                    spawnLocation = GameObject.Find("PlayerReturnPoint6").GetComponent<Transform>();
                else if (PlayerSpawnManager.Instance.prevSceneName == "Playtest7")
                    spawnLocation = GameObject.Find("PlayerReturnPoint7").GetComponent<Transform>();
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
