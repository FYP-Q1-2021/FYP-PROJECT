using UnityEngine;

public enum DialogueTrigger
{
    SPAWN,
    FIRST_GOBLIN_SIGHTED,
    FIRST_GOBLIN_SLAYED,
    COLLISION_WITH_BARRIER,
    CHEST_OPENED,
    BOSS_ROOM,
    BOSS_SPAWNED,
    COLLISION_WITH_DOOR,
    BOSS_DIED
}

public class DialogueEvent : MonoBehaviour
{
    public DialogueTrigger trigger;

    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private Goblin goblin;
    [SerializeField] private OnTriggerEvent goblinViewingPoint;
    [SerializeField] private SphereCollider barrier;
    [SerializeField] private ChestSpawnManager chest;

    void Start()
    {
        if (sceneLoadManager)
            sceneLoadManager.OnSceneFinishedLoading += TriggerDialogue;
        else if (goblin)
        {
            goblin.OnDeath += TriggerDialogue;

            if (goblinViewingPoint)
                goblinViewingPoint.OnTrigger += TriggerDialogue;
        }
        else if (barrier)
        {
            // Subscribe to barrier
        }
        else if (chest)
            chest.OnInteract += TriggerDialogue;
    }

    void OnDestroy()
    {
        if (sceneLoadManager)
            sceneLoadManager.OnSceneFinishedLoading -= TriggerDialogue;
        else if (goblin)
        {
            goblin.OnDeath -= TriggerDialogue;
            if (goblinViewingPoint)
                goblinViewingPoint.OnTrigger -= TriggerDialogue;
        }
        else if (barrier)
        {
            // Unsubscribe to barrier
        }
        else if (chest)
            chest.OnInteract -= TriggerDialogue;
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.ShowDialogue(trigger);
    }
}
