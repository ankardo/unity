using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAllTiles : AOE
{
    public override List<LogicTile> GetArea(List<LogicTile> tiles)
    {
        return tiles;
    }
}
