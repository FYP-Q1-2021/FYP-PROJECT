using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedDialogueEventsManager : MonoBehaviour
{
    public static FinishedDialogueEventsManager Instance { get; private set; }

    public List<string> finishedDialogueEventsList = new List<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddToFinishedDialogueEventsList(GameObject go)
    {
        finishedDialogueEventsList.Add(go.name + " " + SceneManager.GetActiveScene().name);
    }

    public void DeleteFinishedDialogueEventsAfterReload()
    {
        int numOfFinishedDialogueEvents = finishedDialogueEventsList.Count;

        if (numOfFinishedDialogueEvents == 0)
            return;

        DialogueEvent[] dialogueEvents = FindObjectsOfType<DialogueEvent>();
        int numOfDialogueEvents = dialogueEvents.Length;

        for (int i = 0; i < numOfDialogueEvents; ++i)
        {
            for (int j = 0; j < numOfFinishedDialogueEvents; ++j)
            {
                if (string.Compare(dialogueEvents[i].name + " " + SceneManager.GetActiveScene().name, finishedDialogueEventsList[j]) == 0)
                {
                    dialogueEvents[i].UnsubscribeImmediately();
                }
            }
        }
    }
}
