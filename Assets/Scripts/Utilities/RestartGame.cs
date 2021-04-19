using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Persistent");
        SceneManager.LoadScene("DemoScene", LoadSceneMode.Additive);
    }
}
