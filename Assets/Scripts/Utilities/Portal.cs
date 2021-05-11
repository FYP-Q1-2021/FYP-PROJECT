using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string currentRoom = "";
    [SerializeField] string nextRoom = "";
    [SerializeField] SpawnPointData spawnPointData;
    [SerializeField] bool backToPreviousRoom;
    private bool isTriggered;
    private PlayerCharacterController playerCharacterController;

    private SceneTransitionManager sceneTransitionManager;

    void Start()
    {
        playerCharacterController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacterController>();

        sceneTransitionManager = SceneTransitionManager.Instance;
        sceneTransitionManager.OnTransitionFadeFinished += EnableCharacter;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            playerCharacterController.enabled = false;
            if (backToPreviousRoom)
            {
                PlayerSpawnManager.Instance.isReturning = true;
                PlayerSpawnManager.Instance.prevSceneName = currentRoom;
            }
            StartCoroutine("SwitchScene");
            isTriggered = true;
        }
    }

    void OnDisable()
    {
        if(sceneTransitionManager)
            sceneTransitionManager.OnTransitionFadeFinished -= EnableCharacter;
    }

    IEnumerator SwitchScene()
    {
        SceneTransitionManager.Instance.ActivateSceneTransitionFade();
        yield return new WaitForSeconds(SceneTransitionManager.Instance.sceneTransitionCanvasFadeDuration - 0.1f);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(currentRoom));
        Resources.UnloadUnusedAssets();
        SceneManager.LoadSceneAsync(nextRoom, LoadSceneMode.Additive);
        yield return null;
    }

    private void EnableCharacter()
    {
        playerCharacterController.enabled = true;
    }
}
