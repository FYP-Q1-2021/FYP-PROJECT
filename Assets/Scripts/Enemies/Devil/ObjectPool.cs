using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool;
    public List<GameObject> pooledObjects = new List<GameObject>();
    public Transform holder;

    void Start()
    {
        GameObject go;
        for (int i = 0; i < amountToPool; ++i)
        {
            go = Instantiate(objectToPool);
            go.transform.parent = holder;
            go.SetActive(false);
            pooledObjects.Add(go);
        }
    }
    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; ++i)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }
        return null;
    }
}
