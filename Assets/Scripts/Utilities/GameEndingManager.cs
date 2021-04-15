using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameEndingManager : MonoBehaviour
{
    [SerializeField] private Health player;
    [SerializeField] private Canvas inGameCanvas;
    private bool fade;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup endingCanvas;
    private float timer;


    void Start()
    {
        
    }

    void Update()
    {
        if(fade)
        {
            timer += Time.deltaTime;
            endingCanvas.alpha = timer / fadeDuration;
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            inGameCanvas.gameObject.SetActive(false);
            endingCanvas.gameObject.SetActive(true);


            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(player.GetCurrentHealth() < 1f)
        {

        }
    }
}
