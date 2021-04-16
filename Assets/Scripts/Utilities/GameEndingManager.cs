using UnityEngine;

public class GameEndingManager : MonoBehaviour
{
    [SerializeField] private Health player;
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private float fadeDuration = 1f;
    private readonly float fadeSpeed = 1f;
    [SerializeField] private CanvasGroup endingCanvas;
    private float timer;
    private bool ending;

    void Update()
    {
        if(ending)
        {
            timer += fadeSpeed * Time.deltaTime;
            endingCanvas.alpha = timer / fadeDuration;
        }

        if(player.GetCurrentHealth() < 1f && !ending)
        {
            ending = true;

            EnemyManager.Instance.StopAllEnemies();

            inGameCanvas.gameObject.SetActive(false);
            endingCanvas.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
