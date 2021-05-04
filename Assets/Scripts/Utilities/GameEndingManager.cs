using System.Collections;
using UnityEngine;

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

    void Start()
    {
        player.OnDamaged += OnPlayerDeath;
    }

    void OnDisable()
    {
        player.OnDamaged -= OnPlayerDeath;
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
            endingCanvas.alpha = timer / fadeDuration;
            yield return null;
        }
    }
}
