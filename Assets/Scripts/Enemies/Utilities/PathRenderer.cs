using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathRenderer : MonoBehaviour
{
    private List<GameObject> waypoints = new List<GameObject>();
    private int numOfLines;
    public Color color = Color.red;

    void Start()
    {
        foreach (Transform child in transform)
            waypoints.Add(child.gameObject);

        numOfLines = waypoints.Count - 1;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < numOfLines; i++)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
        }
    }
}
