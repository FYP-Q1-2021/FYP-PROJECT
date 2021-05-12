using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private List<Transform> spellEjectionPoints = new List<Transform>();
    private int numOfEjectionPoints;
    private string caster;

    void Start()
    {
        caster = transform.parent.name;
        numOfEjectionPoints = spellEjectionPoints.Count;
    }

    public void Attack()
    {
        for(int i = 0; i < numOfEjectionPoints; ++i)
        {
            GameObject crystal = CrystalPool.Instance.GetPooledObject();
            crystal.transform.position = spellEjectionPoints[i].position;

            Crystal crystalComponent = crystal.GetComponent<Crystal>();

            Vector3 attackDir = spellEjectionPoints[i].forward;

            crystalComponent.SetDirection(attackDir);
            crystalComponent.CasterName(caster);
        }
    }
}
