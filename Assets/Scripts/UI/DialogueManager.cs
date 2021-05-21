using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public List<DialogueData> dialogueDatas = new List<DialogueData>();
    private int dialogueCount;

    [Header("DialogueBox")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float duration = 5f;
    bool isCoroutineRunning;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        dialogueCount = dialogueDatas.Count;
        dialogueBox.SetActive(false);
    }

    public void ShowDialogue(DialogueTrigger trigger)
    {
        for(int i = 0; i < dialogueCount; ++i)
        {
            if(dialogueDatas[i].trigger == trigger)
            {
                text.text = dialogueDatas[i].dialogue;
                if (isCoroutineRunning)
                    StopCoroutine("Dialogue");
                StartCoroutine("Dialogue");
                return;
            }
        }
    }

    IEnumerator Dialogue()
    {
        isCoroutineRunning = true;

        dialogueBox.SetActive(true);
        yield return new WaitForSeconds(duration);
        dialogueBox.SetActive(false);

        isCoroutineRunning = false;
    }
}
