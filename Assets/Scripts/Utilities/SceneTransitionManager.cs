using System.Collections;
using UnityEngine;
using System;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [SerializeField] private CanvasGroup sceneTransitionCanvas;
    public float sceneTransitionCanvasFadeDuration = 1f;
    private readonly float fadeSpeed = 1f;
    private float timer = 0f;

    public event Action OnTransitionFadeFinished;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ActivateSceneTransitionFade()
    {
        sceneTransitionCanvas.gameObject.SetActive(true);
        StartCoroutine("SceneTransitionScreen");
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
        timer = 0f;
        sceneTransitionCanvas.gameObject.SetActive(false);

        OnTransitionFadeFinished?.Invoke();
    }
}
