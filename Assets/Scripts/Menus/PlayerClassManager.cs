using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerClassManager : MonoBehaviour
{
    public static PlayerClassManager Instance { get; private set; }

    public string originalClassName;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        originalClassName = player.name.Substring(0, player.name.Length - 7); // removes (Clone) from name
        player.name = "Player";
        player.SetActive(false); // Reset the camera to fix the post processing effects
        player.SetActive(true);
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName("Persistent"));
    }
}
