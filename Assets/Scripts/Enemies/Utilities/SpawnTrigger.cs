using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    private Health health;
    private bool isPassed;

    void Start()
    {
        health = GameObject.Find("Devil").GetComponent<Health>();
        health.OnDamaged += OnBossDeathEvent;
    }

    void OnDestroy()
    {
        health.OnDamaged -= OnBossDeathEvent;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPassed)
        {
            isPassed = true;
            objectToSpawn.SetActive(true);
        }
    }

    private void OnBossDeathEvent()
    {
        DestroyedObjectManager.Instance.AddToDestroyedObjectsList(gameObject);
    }
}
