using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    private List<GameObject> obstacles = new List<GameObject>();

    private List<GameObject> goalLocations = new List<GameObject>();

    public List<GameObject> Obstacles { get { return obstacles; } }

    public List<GameObject> GoalLocations { get { return goalLocations; } }


    public static GameEnvironment Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();
                instance.goalLocations.AddRange(GameObject.FindGameObjectsWithTag("goal"));
            }
            return instance;
        }
    }
    public void AddObstacles(GameObject gameObject)
    {
        obstacles.Add(gameObject);
    }
    public void RemoveObstacles(GameObject gameObject)
    {
        int index = obstacles.IndexOf(gameObject);
        obstacles.RemoveAt(index);
        GameObject.Destroy(gameObject);
    }

    public GameObject GetRandomGoal()
    {
        int index = Random.Range(0, goalLocations.Count);
        return goalLocations[index];
    }


}
