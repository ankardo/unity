using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpecificTiles : AOE
{
    public int range;
    public int verticalRange;
    [HideInInspector]
    public LogicTile selectorTile;
    public override List<LogicTile> GetArea(List<LogicTile> tiles)
    {
        selectorTile = Selector.instance.tile;
        return Board.instance.Search(selectorTile, SearchType);
    }
    private bool SearchType(LogicTile from, LogicTile to)
    {
        to.distance = from.distance + 1;
        return (from.distance + 1) <= range && Mathf.Abs(to.height - selectorTile.height ) <= verticalRange;
    }
}
