using UnityEngine;
using System;

public class ChestSpawnManager : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private GameObject item;
    private Transform player;
    private InputHandler inputHandler;
    public event Action OnInteract;

    void Start()
    {
        EnemyManager.Instance.OnEnemyDeath += OnLastEnemyDeathEvent;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<Transform>();
        inputHandler = p.GetComponent<InputHandler>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if(distance < radius)
        {
            if(inputHandler.GetInteractKeyDown())
            {
                Instantiate(item, transform.position, Quaternion.identity);
                OnInteract?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
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