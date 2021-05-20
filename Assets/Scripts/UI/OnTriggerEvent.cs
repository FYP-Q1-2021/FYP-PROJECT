using UnityEngine;
using System;

public class OnTriggerEvent : MonoBehaviour
{
    public event Action OnTrigger;

    bool triggered;

    [SerializeField] private bool bossRoom;
    private Devil devil;
    private bool showWarning = true;

    void Start()
    {
        if (bossRoom)
        {
            devil = GameObject.Find("Devil").GetComponent<Devil>();
            devil.OnDeath += DeactivateWarning;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(!triggered && other.name == "Player")
        {
            if(bossRoom && showWarning)
            {
                OnTrigger?.Invoke();
            }
            else if(!bossRoom)
            {
                OnTrigger?.Invoke();
                triggered = true;
            }
        }
    }

    private void OnDestroy()
    {
        if(bossRoom)
            devil.OnDeath -= DeactivateWarning;
    }

    private void DeactivateWarning()
    {
        showWarning = false;
    }
}
