using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public static Unit unit;
    public static Skill chosenSkill;
    public static List<LogicTile> targets;

    public static Item item;

    public static bool hasActed;
    public static bool hasMoved;

    public const int EndTurnChargeTime = 300;
    public const int MoveChargeTime = 100;
    public const int ActionChargeTime = 100;
}
