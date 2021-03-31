using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LerpTest : MonoBehaviour
{

    public GameObject pointA;
    public GameObject pointB;
    public GameObject CObject;
    [Range(0,1)]
    public float lerpValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CObject.transform.position = Vector3.Lerp(pointA.transform.position, pointB.transform.position, lerpValue);
    }
}
