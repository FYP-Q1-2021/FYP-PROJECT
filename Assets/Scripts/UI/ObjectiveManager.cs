using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    public List<DialogueData> objectiveDatas = new List<DialogueData>();
    private int objectiveCount;

    [Header("Objective")]
    [SerializeField] private GameObject objective;
    private TextMeshProUGUI objectiveText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        objectiveCount = objectiveDatas.Count;
        objective.SetActive(false);
        objectiveText = objective.GetComponent<TextMeshProUGUI>();
    }

    public void ShowObjective(DialogueTrigger trigger)
    {
        for (int i = 0; i < objectiveCount; ++i)
        {
            if (objectiveDatas[i].trigger == trigger)
            {
                if(objectiveDatas[i].dialogue == "disable")
                {
                    objective.SetActive(false);
                }
                else
                {
                    objectiveText.text = objectiveDatas[i].dialogue;
                    objective.SetActive(true);
                }
                return;
            }
        }
    }
}
