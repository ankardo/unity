using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeLine : SkillRange
{
    public override bool IsDirectionOriented()
    {
        return true;
    }
    public override List<LogicTile> GetTilesInRange()
    {
        Unit unit = Turn.unit;
        Vector3Int startPos = unit.tile.pos;
        Vector3Int direction = new Vector3Int(0, 0, 0);
        List<LogicTile> returnValue = new List<LogicTile>();

        switch (unit.direction)
        {
            case 'N':
                direction = Vector3Int.up;
                break;
            case 'S':
                direction = Vector3Int.down;
                break;
            case 'E':
                direction = Vector3Int.right;
                break;
            case 'W':
                direction = Vector3Int.left;
                break;
        }
        Vector3Int currentPos = startPos;
        for (int i = 0; i < range; i++)
        {
            currentPos += direction;
            LogicTile tile = Board.GetTile(currentPos);

            if (tile != null && Mathf.Abs(tile.floor.height - unit.tile.floor.height) <= verticalRange)
                returnValue.Add(tile);
        }
        return returnValue;
    }
}
