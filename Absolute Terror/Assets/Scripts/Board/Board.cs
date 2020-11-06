using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Dictionary<Vector3Int, LogicTile> tiles;
    public List<Floor> floors;
    public static Board instance;
    [HideInInspector]
    public Grid grid;
    public List<Tile> highlights;
    public Vector3Int[] directions = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };
    private void Awake()
    {
        tiles = new Dictionary<Vector3Int, LogicTile>(); // the key is the position of the tile
        instance = this;
        grid = GetComponent<Grid>();
    }
    public IEnumerator InitSequence(LoadState loadState)
    {
        yield return StartCoroutine(LoadFloors(loadState));
        yield return null;
        yield return StartCoroutine(ShadowOrdering(loadState));
        yield return null;

    }

    private void CreateTile(Vector3Int pos, Floor floor)
    {
        Vector3 worldPos = grid.CellToWorld(pos);
        worldPos.y += Math.Max(0, floor.tilemap.tileAnchor.y / 2 - 0.25f);
        LogicTile logicTile = new LogicTile(pos, worldPos, floor);
        tiles.Add(pos, logicTile);
    }


    private IEnumerator LoadFloors(LoadState loadState)
    {
        for (int i = 0; i < floors.Count; i++)
        {
            yield return null;
            List<Vector3Int> floorTiles = floors[i].LoadTiles();
            for (int j = 0; j < floorTiles.Count; j++)
            {
                if (!tiles.ContainsKey(floorTiles[j]))
                    CreateTile(floorTiles[j], floors[i]);
            }
        }
    }

    private IEnumerator ShadowOrdering(LoadState loadState)
    {
        foreach (LogicTile logicTile in tiles.Values)
        {
            yield return null;

            int floorIndex = floors.IndexOf(logicTile.floor);
            floorIndex -= 2;

            if (floorIndex >= floors.Count || floorIndex < 0)
                continue;

            Floor floorToCheck = floors[floorIndex];

            Vector3Int pos = logicTile.pos;
            IsNECheck(floorToCheck, logicTile, pos + Vector3Int.right);
            IsNECheck(floorToCheck, logicTile, pos + Vector3Int.up);
            IsNECheck(floorToCheck, logicTile, pos + Vector3Int.right + Vector3Int.up);

        }
    }
    private void IsNECheck(Floor floor, LogicTile logicTile, Vector3Int NEPosition)
    {
        if (floor.tilemap.HasTile(NEPosition))
            logicTile.contentOrder = floor.order;
    }
    public static LogicTile GetTile(Vector3Int pos)
    {
        LogicTile tile;
        instance.tiles.TryGetValue(pos, out tile);

        return tile;
    }
    public void HighlightTiles(List<LogicTile> tiles, int AllianceIndex)
    {
        foreach (LogicTile tile in tiles)
        {
            tile.floor.highlight.SetTile(tile.pos, highlights[AllianceIndex]);
        }
    }
    public void UndoHighlightTiles(List<LogicTile> tiles)
    {
        foreach (LogicTile tile in tiles)
        {
            tile.floor.highlight.SetTile(tile.pos, null);
        }
    }
    public List<LogicTile> Search(LogicTile start, Func<LogicTile, LogicTile, bool> searchType)
    {

        List<LogicTile> searchTiles = new List<LogicTile>();
        searchTiles.Add(start);
        ClearSearch();

        Queue<LogicTile> currentCheck = new Queue<LogicTile>();
        Queue<LogicTile> nextCheck = new Queue<LogicTile>();

        start.distance = 0;
        currentCheck.Enqueue(start);

        while (currentCheck.Count > 0)
        {
            LogicTile currentTile = currentCheck.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                LogicTile nextTile = GetTile(currentTile.pos + directions[i]);
                if (!(nextTile == null || nextTile.distance <= currentTile.distance + 1) && searchType(currentTile, nextTile))
                {
                    nextTile.prev = currentTile;
                    nextCheck.Enqueue(nextTile);
                    searchTiles.Add(nextTile);
                }
                else
                    continue;
            }
            if (currentCheck.Count == 0)
                SwapReference(ref currentCheck, ref nextCheck);
        }
        return searchTiles;
    }
    public void ClearSearch()
    {
        foreach (LogicTile tile in tiles.Values)
        {
            tile.prev = null;
            tile.distance = int.MaxValue;
        }
    }
    private void SwapReference(ref Queue<LogicTile> current, ref Queue<LogicTile> next)
    {
        Queue<LogicTile> temp = current;
        current = next;
        next = temp;

    }

}
