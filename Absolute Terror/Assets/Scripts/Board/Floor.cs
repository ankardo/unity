using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Floor : MonoBehaviour
{
    [HideInInspector]
    public TilemapRenderer tilemapRenderer;
    public int order { get { return tilemapRenderer.sortingOrder; } }
    public int contentOrder;
    [HideInInspector]
    public Tilemap tilemap;

    public Tilemap highlight;
    public int height;

    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
        highlight = transform.parent.GetChild(0).GetComponent<Tilemap>();
    }
    public List<Vector3Int> LoadTiles()
    {
        List<Vector3Int> tiles = new List<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
                tiles.Add(new Vector3Int(pos.x, pos.y, 0));

        }
        return tiles;
    }

}
