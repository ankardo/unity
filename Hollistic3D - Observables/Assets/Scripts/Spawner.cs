using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
   public GameObject prefab;
   public Terrain terrain;
   private TerrainData terrainData;
   private void Start()
   {
      terrainData = terrain.terrainData;
      InvokeRepeating("SpawnObjectInRandomPos", 5, 0.1f);
      
   }
   private void SpawnObjectInRandomPos()
   {
      int x = (int)Random.Range(0, terrainData.size.x);
      int z = (int)Random.Range(0, terrainData.size.z);
      Vector3 pos = new Vector3(x, 0, z);
      pos.y = terrain.SampleHeight(pos) + 10;
      GameObject gameObject = Instantiate(prefab, pos, Quaternion.identity);
      
   }
}
