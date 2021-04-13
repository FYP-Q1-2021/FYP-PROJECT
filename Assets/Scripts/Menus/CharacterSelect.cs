using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SelectCharacter()
    {
        //SceneManager.LoadScene("DemoScene");
        //SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        //SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent");
        SceneManager.LoadScene("DemoScene", LoadSceneMode.Additive);
        //StartCoroutine("LoadNextScene");
        //SceneManager.LoadScene("DemoScene", LoadSceneMode.Additive);
        //PlayerSpawnManager.Instance.player.transform.SetPositionAndRotation(PlayerSpawnManager.Instance.spawnPoints[0].position, Quaternion.Euler(PlayerSpawnManager.Instance.spawnPoints[0].rotation));
    }

    //IEnumerator LoadNextScene()
    //{
    //SceneManager.LoadScene("Persistent");
    //yield return new WaitForSeconds(1f);
    //PlayerSpawnManager.Instance.player.transform.SetPositionAndRotation(PlayerSpawnManager.Instance.spawnPoints[0].position, Quaternion.Euler(PlayerSpawnManager.Instance.spawnPoints[0].rotation));
    //        SceneManager.LoadScene("DemoScene", LoadSceneMode.Additive);
    //yield return new WaitForSeconds(1f);
    //}
}
