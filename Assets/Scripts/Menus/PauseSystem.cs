using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{

    public static bool isPaused = false;

    public GameObject pauseMenu;
    public GameObject inGameUI;
    
    
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else if(!isPaused)
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        //GameObject.Find("Dialogue Box").SetActive(false);
        pauseMenu.SetActive(true);
        inGameUI.SetActive(false);
        isPaused = true;
        Time.timeScale = 0f;

    }

    public void ResumeGame()
    {
        //GameObject.Find("Dialogue Box").SetActive(true);
        pauseMenu.SetActive(false);
        inGameUI.SetActive(true);
        isPaused = false;
        Time.timeScale = 1f;
    }
}
