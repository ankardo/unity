using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTile
{
    public Vector3Int pos;
    public Vector3 worldPos;
    public GameObject content;
    public Floor floor;
    public int contentOrder;

    #region pathfinding
    public LogicTile prev;
    public float distance;
    public int height { get { return floor.height;} }
    #endregion

    public LogicTile() { }

    public LogicTile(Vector3Int cellPos, Vector3 worldPosition, Floor tempFloor)
    {
        pos = cellPos;
        worldPos = worldPosition;
        floor = tempFloor;
        contentOrder = tempFloor.contentOrder;
    }

    public char GetDirection(LogicTile nextTile)
    {
        return GetDirection(nextTile.pos);
    }
    public char GetDirection(Vector3Int nextTile)
    {
        if (pos.y < nextTile.y)
            return 'N';
        if (pos.y > nextTile.y)
            return 'S';
        if (pos.x < nextTile.x)
            return 'E';
        if (pos.x > nextTile.x)
            return 'W';

        return ' ';
    }

}
