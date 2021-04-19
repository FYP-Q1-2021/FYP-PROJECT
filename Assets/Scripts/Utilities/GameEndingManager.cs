using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndingManager : MonoBehaviour
{
    public static GameEndingManager Instance { get; private set; }

    [SerializeField] private Health player;
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private float fadeDuration = 1f;
    private readonly float fadeSpeed = 1f;
    [SerializeField] private CanvasGroup endingCanvas;
    private float timer;
    public bool winEnding;
    public bool loseEnding;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Need to refactor
    void Update()
    {
        if(winEnding || loseEnding)
        {
            timer += fadeSpeed * Time.deltaTime;
            endingCanvas.alpha = timer / fadeDuration;
        }

        if((player.GetCurrentHealth() < 1f && !loseEnding) || winEnding)
        {
            loseEnding = true;
            winEnding = false;

            EnemyManager.Instance.StopAllEnemies();

            inGameCanvas.gameObject.SetActive(false);
            endingCanvas.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
