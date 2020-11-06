using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESelectorTile : AOE
{
    public override List<LogicTile> GetArea(List<LogicTile> tiles)
    {
        List<LogicTile> returnValue = new List<LogicTile>();
        returnValue.Add(Selector.instance.tile);
        return returnValue;
    }
}
