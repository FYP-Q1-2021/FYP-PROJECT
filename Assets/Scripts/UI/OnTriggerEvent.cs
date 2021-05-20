using UnityEngine;
using System;

public class OnTriggerEvent : MonoBehaviour
{
    public event Action OnTrigger;

    bool triggered;

    void OnTriggerEnter(Collider other)
    {
        if(!triggered && other.name == "Player")
        {
            OnTrigger?.Invoke();
            triggered = true;
        }
    }
}
