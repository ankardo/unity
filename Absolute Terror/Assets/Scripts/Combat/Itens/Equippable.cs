using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equippable : Item
{
    public override void Use(Unit unit)
    {
        foreach(Modifier modifier in GetComponents<Modifier>())
        {
            modifier.Activate(unit);
        }
    }
}
