using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject spherePrefab;
    private void Update()
    {
        if (Random.Range(0, 100) < 10)
            Instantiate(cubePrefab, this.transform.position, Quaternion.identity);
        else if (Random.Range(0, 100) < 10)
                Instantiate(spherePrefab, this.transform.position, Quaternion.identity);
    }
}
