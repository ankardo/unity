using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject prefab;

    private void Update()
    {
        if (Random.Range(0, 100) < 5)
        {
            GameObject asteroid = Pool.instance.Get("Asteroid");
            if (asteroid != null)
            {
                asteroid.transform.position = this.transform.position + new Vector3(Random.Range(-10, 10), 0, 0);
                asteroid.SetActive(true);
            }
        }
    }
}
