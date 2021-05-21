using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{

    public static bool isPaused = false;

    public GameObject pauseMenu;
    
    
    void Start()
    {
        pauseMenu.SetActive(false);

    }
    // Update is called once per frame
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

        Debug.Log(Time.timeScale);
    }

    public void PauseGame()
    {
        //GameObject.Find("Dialogue Box").SetActive(false);
        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;

    }

    public void ResumeGame()
    {
        //GameObject.Find("Dialogue Box").SetActive(true);
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

    }
}
