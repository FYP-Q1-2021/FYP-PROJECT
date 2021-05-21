using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public List<GameObject> classes = new List<GameObject>();

    public void Restart()
    {
        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");
        Destroy(currentPlayer);
        StartCoroutine("Restarting");
    }

    IEnumerator Restarting()
    {
        yield return new WaitForEndOfFrame(); // Wait for the current player to be destroyed before instantiating a new one

        for (int i = 0; i < classes.Count; ++i)
        {
            if (classes[i].name == PlayerClassManager.Instance.originalClassName)
            {
                GameObject p = Instantiate(classes[i]);
                DontDestroyOnLoad(p);
                break;
            }
        }

        SceneManager.LoadScene("Persistent");
        SceneManager.LoadScene("Playtest", LoadSceneMode.Additive);
    }
}
