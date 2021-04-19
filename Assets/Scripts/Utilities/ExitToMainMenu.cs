using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMainMenu : MonoBehaviour
{
    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
