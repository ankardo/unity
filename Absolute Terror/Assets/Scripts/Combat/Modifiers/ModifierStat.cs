using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierStat : Modifier
{
    public StatEnum statEnum;
    protected override void Modify(object args)
    {
        Stat stat = (Stat)args;
        stat.currentValue += (int)value;
    }

    public override void Activate(Unit unit)
    {
        base.Activate(unit);
        host.stats[statEnum].statModifiers += Modify;
        host.UpdateStat(statEnum);
    }
    public override void Deactivate()
    {
        host.stats[statEnum].statModifiers -= Modify;
        host.UpdateStat(statEnum);
    }
}
