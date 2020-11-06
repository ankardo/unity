using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeSelf : SkillRange
{
    private void Awake() {
        range = 1;
        verticalRange = 1;
    }
    public override List<LogicTile> GetTilesInRange()
    {
        List<LogicTile> returnValue = new List<LogicTile>();
        returnValue.Add(Turn.unit.tile);

        return returnValue;
    }
}
