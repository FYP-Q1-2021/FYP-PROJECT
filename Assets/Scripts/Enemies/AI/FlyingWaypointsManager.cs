using System.Collections.Generic;
using UnityEngine;

public class FlyingWaypointsManager : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    public int currentWaypoint = 0;
    private int lastWaypointIndex;
    public bool endPointReached;
    [SerializeField] private bool cycle;
    [SerializeField] private bool reverseDir;

    private float movementSpeed;
    private float rotationSpeed;

    private readonly float waypointOffset = 0.5f;

    void Start()
    {
        movementSpeed = GetComponent<Imp>().movementSpeed;
        rotationSpeed = GetComponent<Imp>().rotationSpeed;
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex];
    }

    void Update()
    {
        float distanceToTurn = rotationSpeed * Time.deltaTime;
        Vector3 targetDir = targetWaypoint.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, distanceToTurn);

        float distanceToMove = movementSpeed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        CheckDistanceToWaypoint(distance);
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, distanceToMove);
    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= waypointOffset)
            UpdateTargetWaypoint();
    }

    void UpdateTargetWaypoint()
    {
        if (!reverseDir)
        {
            if (targetWaypointIndex < lastWaypointIndex)
            {
                currentWaypoint = targetWaypointIndex;
                ++targetWaypointIndex;
            }
            else
            {
                reverseDir = true;
                endPointReached = true;
                currentWaypoint = targetWaypointIndex;
                targetWaypointIndex = waypoints.Count - 2;
            }
        }
        else
        {
            if (targetWaypointIndex > 0)
            {
                currentWaypoint = targetWaypointIndex;
                --targetWaypointIndex;
            }
            else
            {
                reverseDir = false;
                endPointReached = true;
                currentWaypoint = 0;
                targetWaypointIndex = 1;
            }
        }

        targetWaypoint = waypoints[targetWaypointIndex];
    }

    public void FindNearestWaypoint()
    {
        int smallestWaypointIndex = 0;
        float smallestDist = Vector3.Distance(transform.position, waypoints[smallestWaypointIndex].transform.position);
        for (int i = 1; i < waypoints.Count; ++i)
        {
            float dist = Vector3.Distance(transform.position, waypoints[i].transform.position);
            if (dist < smallestDist)
            {
                smallestDist = dist;
                smallestWaypointIndex = i;
            }
        }
        targetWaypoint = waypoints[smallestWaypointIndex];
    }
}
