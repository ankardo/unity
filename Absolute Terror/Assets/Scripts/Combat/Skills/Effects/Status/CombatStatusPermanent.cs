using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatusPermanent : CombatStatus
{
    public override void Effect()
    {   
        Modifier[] modifiers = GetComponents<Modifier>();
        foreach (Modifier modifier in modifiers)
        {
            modifier.Activate(unit);
        }
    }
}
