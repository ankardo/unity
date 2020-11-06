using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatLog
{
    public static List<string> log = new List<string>();

    public static void CheckIfActive()
    {
        foreach (Unit unit in StateMachineController.instance.units)
        {
            unit.active = (unit.stats[StatEnum.HP].currentValue > 0);
        }
    }
    public static bool CheckIfCombatIsOver()
    {

        foreach (Alliance alliance in MapLoader.instance.alliances)
        {
            ChangeActiveAlliances(alliance);
        }

        int activeAlliances = 0;
        for (int i = 0; i < MapLoader.instance.alliances.Count; i++)
        {
            activeAlliances += AllianceIsActive(MapLoader.instance.alliances[i]);
        }
        return (activeAlliances == 1);
    }
    private static void ChangeActiveAlliances(Alliance alliance)
    {
        for (int i = 0; i < alliance.units.Count; i++)
        {
            Unit currentUnit = alliance.units[i];
            alliance.active = currentUnit.active;
        }

    }
    private static int AllianceIsActive(Alliance alliance)
    {
        for (int i = 0; i < alliance.units.Count; i++)
        {
            Unit currentUnit = alliance.units[i];
            if (currentUnit.active)
                return 1;
        }
        return 0;
    }
    public static void PrintCombatLog()
    {
        for (int i = 0; i < log.Count; i++)
            Debug.Log(log[i]);

    }
}
