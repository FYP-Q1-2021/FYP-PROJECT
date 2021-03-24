using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsManager
{
    private static WaypointsManager instance;
    private List<GameObject> goblinWaypoints = new List<GameObject>();

    public static WaypointsManager Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new WaypointsManager();
                instance.goblinWaypoints.AddRange(GameObject.FindGameObjectsWithTag("GoblinWaypoint"));
            }
            return instance;
        }
    }

    public List<GameObject> GetGoblinWaypoints()
    {
        return goblinWaypoints;
    }
}
