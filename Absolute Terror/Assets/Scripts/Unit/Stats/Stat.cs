using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void StatModifier(Stat stat);
[System.Serializable]
public class Stat 
{
    public StatEnum type;
    public int baseValue;
    public int currentValue;
    public float growth;
    public const int MaxStat = 100;
    public StatModifier statModifiers;

}

