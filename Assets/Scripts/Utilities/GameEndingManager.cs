using System.Collections;
using UnityEngine;

public class GameEndingManager : MonoBehaviour
{
    public static GameEndingManager Instance { get; private set; }

    [SerializeField] private Health player;
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private float endingCanvasFadeDuration = 1f;
    private readonly float fadeSpeed = 1f;
    [SerializeField] private CanvasGroup endingCanvas;
    [SerializeField] private CanvasGroup winCanvas;
    private float timer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        player.OnDamaged += OnPlayerDeath;
    }

    void OnDisable()
    {
        player.OnDamaged -= OnPlayerDeath;
    }

    public void WinEnding()
    {
        inGameCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine("WinScreen");
    }

    private void OnPlayerDeath()
    {
        if(player.GetCurrentHealth() < 1f)
        {
            EnemyManager.Instance.StopAllEnemies();

            inGameCanvas.gameObject.SetActive(false);
            endingCanvas.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            StartCoroutine("EndingScreen");
        }
    }

    IEnumerator EndingScreen()
    {
        while(endingCanvas.alpha < 1f)
        {
            timer += fadeSpeed * Time.deltaTime;
            endingCanvas.alpha = timer / endingCanvasFadeDuration;
            yield return null;
        }
    }

    IEnumerator WinScreen()
    {
        while (winCanvas.alpha < 1f)
        {
            timer += fadeSpeed * Time.deltaTime;
            winCanvas.alpha = timer / endingCanvasFadeDuration;
            yield return null;
        }
    }
}
