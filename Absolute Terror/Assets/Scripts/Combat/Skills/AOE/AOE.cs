using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AOE : MonoBehaviour
{
    public abstract List<LogicTile> GetArea(List<LogicTile> tiles);
}
