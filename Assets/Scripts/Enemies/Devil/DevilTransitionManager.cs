using System.Collections.Generic;
using UnityEngine;
using System;

public class DevilTransitionManager : MonoBehaviour
{
    public List<GameObject> impsGO = new List<GameObject>();
    public List<Imp> imps = new List<Imp>();
    private int amountOfDeadImps = 0;

    public event Action OnTransitionToPhase3;

    void Awake()
    {
        ImpPool.Instance.OnObjectsPoolingFinished += Initialize; // Subscribe
    }

    private void OnDisable()
    {
        for(int i = 0; i < imps.Count; ++i)
        {
            imps[i].OnDeath -= OnImpDeathEvent;
        }

        ImpPool.Instance.OnObjectsPoolingFinished -= Initialize; // Unsubscribe
    }

    private void OnImpDeathEvent()
    {
        ++amountOfDeadImps;
        if(amountOfDeadImps == ImpPool.Instance.amountToPool)
            OnTransitionToPhase3?.Invoke(); // Notify Devil
    }

    private void Initialize()
    {
        impsGO.AddRange(ImpPool.Instance.pooledObjects);

        int count = impsGO.Count;
        for (int i = 0; i < count; ++i)
        {
            imps.Add(impsGO[i].GetComponent<Imp>());
            imps[i].OnDeath += OnImpDeathEvent;
        }
    }
}
