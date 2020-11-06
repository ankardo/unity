using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeCone : SkillRange
{
    public override bool IsDirectionOriented()
    {
        return true;
    }
    public override List<LogicTile> GetTilesInRange()
    {
        Unit unit = Turn.unit;
        Vector3Int pos = unit.tile.pos;
        List<LogicTile> returnValue = new List<LogicTile>();
        int lateral = 1;
        for (int i = 1; i <= range; i++)
        {
            int min = -(lateral / 2);
            int max = (lateral / 2);
            for (int j = min; j <= max; j++)
            {
                Vector3Int next = GetNext(unit.direction, pos, i, j);
                LogicTile tile = Board.GetTile(next);
                if (ValidTile(tile))
                    returnValue.Add(tile);
            }
            lateral += 2;
        }

        return returnValue;
    }
    private bool ValidTile(LogicTile tile)
    {
        return (tile != null && Mathf.Abs(tile.floor.height - Turn.unit.tile.floor.height) <= verticalRange);
    }

    private Vector3Int GetNext(char orientation, Vector3Int pos, int arg1, int arg2)
    {
        Vector3Int next = Vector3Int.zero;
        switch (orientation)
        {
            case 'N':
                next = new Vector3Int(pos.x + arg2, pos.y + arg1, 0);
                break;
            case 'S':
                next = new Vector3Int(pos.x + arg2, pos.y - arg1, 0);
                break;
            case 'E':
                next = new Vector3Int(pos.x + arg1, pos.y + arg2, 0);
                break;
            case 'W':
                next = new Vector3Int(pos.x - arg1, pos.y + arg2, 0);
                break;
        }
        return next;
    }
}