using UnityEngine;

public class ChestSpawnManager : MonoBehaviour
{
    void Start()
    {
        EnemyManager.Instance.OnEnemyDeath += OnLastEnemyDeathEvent;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EnemyManager.Instance.OnEnemyDeath -= OnLastEnemyDeathEvent;
    }

    private void OnLastEnemyDeathEvent()
    {
        if (EnemyManager.Instance.numOfEnemies == 0)
            gameObject.SetActive(true);
    }
}
