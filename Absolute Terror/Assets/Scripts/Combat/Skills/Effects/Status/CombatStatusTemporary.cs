using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatusTemporary : CombatStatus
{
    protected override void OnDisable(){
        base.OnDisable();
        unit.OnTurnBegin -= DurationCounter;
    }
    public override void Effect()
    {
        unit.OnTurnBegin += DurationCounter;
        Modifier[] modifiers = GetComponents<Modifier>();
        foreach (Modifier modifier in modifiers)
        {
            modifier.Activate(unit);
        }
    }
    private void DurationCounter()
    {
        duration--;
        if (duration <= 0){
            unit.OnTurnBegin -= DurationCounter;
            Destroy(this.gameObject);
        }
    }
}
