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
    [SerializeField] private CanvasGroup sceneTransitionCanvas;
    public float sceneTransitionCanvasFadeDuration = 1f;
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

    public void ActivateSceneTransitionFade()
    {
        sceneTransitionCanvas.gameObject.SetActive(true);
        StartCoroutine("SceneTransitionScreen");
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

    IEnumerator SceneTransitionScreen()
    {
        while (sceneTransitionCanvas.alpha < 1f)
        {
            timer += fadeSpeed * Time.deltaTime;
            sceneTransitionCanvas.alpha = timer / sceneTransitionCanvasFadeDuration;
            yield return null;
        }
        while (sceneTransitionCanvas.alpha > 0f)
        {
            timer -= fadeSpeed * Time.deltaTime;
            sceneTransitionCanvas.alpha = timer / sceneTransitionCanvasFadeDuration;
            yield return null;
        }
        sceneTransitionCanvas.gameObject.SetActive(false);
    }
}
