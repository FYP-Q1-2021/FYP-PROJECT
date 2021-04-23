using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] GameObject crystalPrefab;
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
            GameObject crystal = Instantiate(crystalPrefab, spellEjectionPoints[i].position, Quaternion.identity);
            Vector3 attackDir = spellEjectionPoints[i].forward;
            crystal.GetComponent<Crystal>().SetDirection(attackDir);
        }
    }
}
