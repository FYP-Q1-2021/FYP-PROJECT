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

/// <summary>
/// How to use:
/// - Drag a DialogueEventHandler prefab into the scene for each event
/// - Set the trigger and the corresponding object
/// - Make sure there is only 1 object assigned to each prefab
/// </summary>
public class DialogueEvent : MonoBehaviour
{
    public DialogueTrigger trigger;

    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private Goblin goblin;
    [SerializeField] private OnTriggerEvent triggerEvent;
    [SerializeField] private OnBarrierTriggerEvent barrier;
    [SerializeField] private ChestSpawnManager chest;
    [SerializeField] private SpawnTrigger bossSpawnTrigger;
    [SerializeField] private Devil devil;

    void Start()
    {
        if (sceneLoadManager)
            sceneLoadManager.OnSceneFinishedLoading += TriggerDialogue;
        else if (goblin)
            goblin.OnDeath += TriggerDialogue;
        else if (triggerEvent)
            triggerEvent.OnTrigger += TriggerDialogue;
        else if (barrier)
            barrier.OnTrigger += TriggerDialogue;
        else if (chest)
            chest.OnInteract += TriggerDialogue;
        else if (bossSpawnTrigger)
            bossSpawnTrigger.OnSpawnBoss += TriggerDialogue;
        else if (devil)
            devil.OnDeath += TriggerDialogue;
    }

    void OnDestroy()
    {
        if (sceneLoadManager)
            sceneLoadManager.OnSceneFinishedLoading -= TriggerDialogue;
        else if (goblin)
            goblin.OnDeath -= TriggerDialogue;
        else if (triggerEvent)
            triggerEvent.OnTrigger -= TriggerDialogue;
        else if (barrier)
            barrier.OnTrigger -= TriggerDialogue;
        else if (chest)
            chest.OnInteract -= TriggerDialogue;
        else if (bossSpawnTrigger)
            bossSpawnTrigger.OnSpawnBoss += TriggerDialogue;
        else if (devil)
            devil.OnDeath -= TriggerDialogue;
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.ShowDialogue(trigger);
        ObjectiveManager.Instance.ShowObjective(trigger);
    }
}
