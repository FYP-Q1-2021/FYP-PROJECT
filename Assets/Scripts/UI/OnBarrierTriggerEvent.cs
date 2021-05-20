using UnityEngine;
using System;

public class OnBarrierTriggerEvent : MonoBehaviour
{
    private float radius;
    private Transform player;
    private bool showDialogue;

    public event Action OnTrigger;

    void Start()
    {
        radius = transform.localScale.z / 2 + 2f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (!showDialogue)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < radius)
            {
                showDialogue = true;
                OnTrigger?.Invoke();
            }
        }
    }
}
