using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeInfinite : SkillRange
{

    public override List<LogicTile> GetTilesInRange()
    {
        return new List<LogicTile>(Board.instance.tiles.Values);
    }
}
