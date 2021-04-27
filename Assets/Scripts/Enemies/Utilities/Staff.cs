using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private List<Transform> spellEjectionPoints = new List<Transform>();
    private int numOfEjectionPoints;

    void Start()
    {
        numOfEjectionPoints = spellEjectionPoints.Count;
    }

    public void Attack()
    {
        for(int i = 0; i < numOfEjectionPoints; ++i)
        {
            GameObject crystal = CrystalPool.Instance.GetPooledObject();
            crystal.transform.position = spellEjectionPoints[i].position;
            Vector3 attackDir = spellEjectionPoints[i].forward;
            crystal.GetComponent<Crystal>().SetDirection(attackDir);
        }
    }
}
