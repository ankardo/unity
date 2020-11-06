using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillRange : MonoBehaviour
{
    public int range = 1;
    public int verticalRange = 0;
    [HideInInspector]
    public virtual bool IsDirectionOriented()
    {
        return false;
    }
    public virtual char GetDirection()
    {
        if (Turn.targets[0].pos == Turn.unit.tile.pos)
            return Turn.unit.direction;
        return Turn.unit.tile.GetDirection(Turn.targets[0]);
    }
    public abstract List<LogicTile> GetTilesInRange();
}
