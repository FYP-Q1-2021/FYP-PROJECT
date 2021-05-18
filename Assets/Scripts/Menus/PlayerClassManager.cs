using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerClassManager : MonoBehaviour
{
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.name = "Player";
        player.SetActive(false); // Reset the camera to fix the post processing effects
        player.SetActive(true);
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName("Persistent"));
    }
}
