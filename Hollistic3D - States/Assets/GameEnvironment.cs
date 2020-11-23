using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class GameEnvironment 
{
    private static GameEnvironment instance;
    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints { get { return checkpoints; } }

    private List<GameObject> safeZones = new List<GameObject>();
    public List<GameObject> SafeZones { get { return safeZones; } }
    public static GameEnvironment Singleton
    {
        get
        {
            if (instance == null)
            {

                instance = new GameEnvironment();

                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                instance.checkpoints = instance.Checkpoints.OrderBy(waypoint => waypoint.name).ToList();

                instance.SafeZones.AddRange(GameObject.FindGameObjectsWithTag("Safe"));
                instance.safeZones = instance.SafeZones.OrderBy(waypoint => waypoint.name).ToList();
                
            }
            return instance;
        }
    }

}
