using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointsManager : MonoBehaviour
{
    public List<GameObject> waypoints = new List<GameObject>();
    public int targetWaypoint = 0;
    public int currentWaypoint = 0;
    private NavMeshAgent agent;
    [SerializeField] private bool reverseDir;
    public bool endPointReached;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (gameObject.CompareTag("Goblin"))
            waypoints.AddRange(GameObject.FindGameObjectsWithTag("GoblinWaypoint"));
        else if (gameObject.CompareTag("Imp"))
            waypoints.AddRange(GameObject.FindGameObjectsWithTag("ImpWaypoint"));
        agent.destination = waypoints[targetWaypoint].transform.position;
    }

    void Update()
    {
        // Choose the next destination point when the agent gets close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    private void GotoNextPoint()
    {
        if (!reverseDir)
        {
            if (targetWaypoint < waypoints.Count - 1)
            {
                currentWaypoint = targetWaypoint;
                ++targetWaypoint;
                agent.destination = waypoints[targetWaypoint].transform.position;
            }
            else
            {
                reverseDir = true;
                endPointReached = true;
                currentWaypoint = targetWaypoint;
                targetWaypoint = waypoints.Count - 2;
                agent.destination = waypoints[targetWaypoint].transform.position;
            }
        }
        else
        {
            if (targetWaypoint > 0)
            {
                currentWaypoint = targetWaypoint;
                --targetWaypoint;
                agent.destination = waypoints[targetWaypoint].transform.position;
            }
            else
            {
                reverseDir = false;
                endPointReached = true;
                currentWaypoint = 0;
                targetWaypoint = 1;
                agent.destination = waypoints[targetWaypoint].transform.position;
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
        targetWaypoint = smallestWaypointIndex;
    }
}
