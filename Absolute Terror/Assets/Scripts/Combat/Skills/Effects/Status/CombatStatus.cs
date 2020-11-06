using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatStatus : MonoBehaviour
{
    [HideInInspector]
    public Unit unit;
    [HideInInspector]
    public int duration;
    public abstract void Effect();
    protected virtual void OnDisable()
    {
        Modifier[] modifiers = GetComponents<Modifier>();
        foreach (Modifier modifier in modifiers)
        {
            modifier.Deactivate();
        }
    }
    public void SetModifiers(int value)
    {
        Modifier[] modifiers = GetComponents<Modifier>();
        foreach (Modifier modifier in modifiers)
        {
            modifier.value = value;
        }
    }
    public void Stack(int receivedDuration, int value)
    {
        duration += receivedDuration;
        Modifier[] modifiers = GetComponents<Modifier>();
        foreach (Modifier modifier in modifiers)
        {
            modifier.value = value;
        }
    }


}
