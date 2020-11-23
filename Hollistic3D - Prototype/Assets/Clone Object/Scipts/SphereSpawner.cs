using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    private void Update()
    {
        if (Random.Range(0, 100) < 10)
            ProcSphere.Clone(this.transform.position);
    }
}
