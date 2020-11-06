using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeConstant : SkillRange
{
    bool SearchType(LogicTile from, LogicTile to)
    {
        to.distance = from.distance + 1;
        return (from.distance + 1) <= range && Mathf.Abs(to.height - Turn.unit.tile.height) <= verticalRange;
    }

    public override List<LogicTile> GetTilesInRange()
    {
        return Board.instance.Search(Turn.unit.tile, SearchType);
    }
}
