using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    private Transform player;

    [SerializeField] GameObject crystalPrefab;
    [SerializeField] private List<Transform> spellEjectionPoints = new List<Transform>();
    private int numOfEjectionPoints;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        numOfEjectionPoints = spellEjectionPoints.Count;
    }

    public void Attack()
    {
        for(int i = 0; i < numOfEjectionPoints; ++i)
        {
            GameObject crystal = Instantiate(crystalPrefab, spellEjectionPoints[i].position, Quaternion.identity);
            Vector3 attackDir = (player.transform.position - spellEjectionPoints[i].position).normalized;
            crystal.GetComponent<Crystal>().SetDirection(attackDir);
        }
    }
}
