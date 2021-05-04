using UnityEngine;

public class ChestSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject item;

    void Start()
    {
        EnemyManager.Instance.OnEnemyDeath += OnLastEnemyDeathEvent;
    }

    void OnDisable()
    {
        EnemyManager.Instance.OnEnemyDeath -= OnLastEnemyDeathEvent;
    }

    private void OnLastEnemyDeathEvent()
    {
        if(EnemyManager.Instance.numOfEnemies == 0)
            Instantiate(item, transform.position, Quaternion.identity);
    }
}
