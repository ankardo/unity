using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Blockers : MonoBehaviour
{
    public static Blockers instance;
    
    private void Awake()
    {
        instance = this;
        GetComponent<TilemapRenderer>().enabled = false;
    }
    public List<Vector3Int> GetBlockedTiles()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        List<Vector3Int> blockedTiles = new List<Vector3Int>();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
                blockedTiles.Add(new Vector3Int(pos.x, pos.y, 0));
        
        }
        return blockedTiles;
    }
}
