using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectInflictStatus : SkillEffect
{
    public CombatStatus combatStatus;
    public string combatStatusName;
    public int combatStatusValue;
    public Color combatStatusColor;
    public int duration;
    
    public override int Predict(Unit target)
    {
        return 0;
    }
    public override void Apply(Unit target)
    {
        Transform holder = target.transform.Find("Status");
        Transform stack = holder.Find(combatStatusName);
        if(stack != null)
            Stack(stack);
        else
            InstantiateNew(holder, target);
    }
    private void InstantiateNew(Transform holder, Unit target)
    {
        CombatStatus combatStatusInstance = Instantiate(combatStatus, holder.position, Quaternion.identity, holder);
        combatStatusInstance.name = combatStatusName;
        combatStatusInstance.SetModifiers(combatStatusValue);
        combatStatusInstance.unit = target;
        CombatText.instance.PopText(target, combatStatusName, combatStatusColor);
        combatStatusInstance.duration = duration;
        combatStatusInstance.Effect();
    }
    private void Stack(Transform stack)
    {
        CombatStatus stackedStatus = stack.GetComponent<CombatStatus>();
        stackedStatus.Stack(duration, combatStatusValue);
    }
}
