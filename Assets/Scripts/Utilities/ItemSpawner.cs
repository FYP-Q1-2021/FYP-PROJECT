using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject item;

    public void Spawn()
    {
        Instantiate(item, transform.position, Quaternion.identity);
    }
}
