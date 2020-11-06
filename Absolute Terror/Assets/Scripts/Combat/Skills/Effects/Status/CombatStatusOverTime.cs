using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatusOverTime : CombatStatus
{

    public override void Effect()
    {
        unit.OnTurnBegin += OverTimeEffect;
    }
    protected override void OnDisable()
    {
        unit.OnTurnBegin -= OverTimeEffect;
    }
    private void OverTimeEffect()
    {
        duration--;

        ModifierStat[] modifiers = GetComponents<ModifierStat>();
        foreach (ModifierStat modifier in modifiers)
        {
            unit.SetStat(modifier.statEnum, (int)modifier.value);
        }
        if (duration <= 0)
            Destroy(this.gameObject);
    }
}
