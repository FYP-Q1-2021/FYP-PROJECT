using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointsManager : MonoBehaviour
{
    public List<GameObject> waypoints = new List<GameObject>();
    
    private NavMeshAgent agent;
    
    public int targetWaypointIndex = 0;
    public int currentWaypoint = 0;
    private int lastWaypointIndex;
    public bool endPointReached;
    [SerializeField] private bool cycle;
    [SerializeField] private bool reverseDir;

    private readonly float waypointOffset = 0.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastWaypointIndex = waypoints.Count - 1;
        agent.destination = waypoints[targetWaypointIndex].transform.position;
    }

    void Update()
    {
        // Choose the next destination point when the agent gets close to the current one.
        if (!agent.pathPending && agent.remainingDistance < waypointOffset)
            UpdateTargetWaypoint();
    }

    private void UpdateTargetWaypoint()
    {
        if (!reverseDir)
        {
            if (targetWaypointIndex < lastWaypointIndex)
            {
                currentWaypoint = targetWaypointIndex;
                ++targetWaypointIndex;
                agent.destination = waypoints[targetWaypointIndex].transform.position;
            }
            else
            {
                reverseDir = true;
                endPointReached = true;
                currentWaypoint = targetWaypointIndex;
                targetWaypointIndex = waypoints.Count - 2;
                agent.destination = waypoints[targetWaypointIndex].transform.position;
            }
        }
        else
        {
            if (targetWaypointIndex > 0)
            {
                currentWaypoint = targetWaypointIndex;
                --targetWaypointIndex;
                agent.destination = waypoints[targetWaypointIndex].transform.position;
            }
            else
            {
                reverseDir = false;
                endPointReached = true;
                currentWaypoint = 0;
                targetWaypointIndex = 1;
                agent.destination = waypoints[targetWaypointIndex].transform.position;
            }
        }
    }

    public void FindNearestWaypoint()
    {
        int smallestWaypointIndex = 0;
        float smallestDist = Vector3.Distance(transform.position, waypoints[smallestWaypointIndex].transform.position);
        for(int i = 1; i < waypoints.Count; ++i)
        {
            float dist = Vector3.Distance(transform.position, waypoints[i].transform.position);
            if(dist < smallestDist)
            {
                smallestDist = dist;
                smallestWaypointIndex = i;
            }
        }
        targetWaypointIndex = smallestWaypointIndex;
    }
}
