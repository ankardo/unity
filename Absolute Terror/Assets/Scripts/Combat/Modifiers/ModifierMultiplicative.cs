using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MultiplicativeTypeEnum
{
    attacker,
    defender
}
public class ModifierMultiplicative : Modifier
{
    public MultiplicativeTypeEnum type;
    public ModifierCondition condition;
    protected override void Modify(object args)
    {
        MultiplicativeForms forms = (MultiplicativeForms)args;
        if (IsValidModifierType() && (condition == null || condition.Validate(forms))){

            if(type == MultiplicativeTypeEnum.attacker) 
                forms.currentValue += (int)value;
            else
                forms.currentValue -= (int)value;
        }
    }

    public override void Activate(Unit unit)
    {
        base.Activate(unit);
        host.stats.multiplicativeModifiers += Modify;
    }
    public override void Deactivate()
    {
        host.stats.multiplicativeModifiers -= Modify;
    }
    private bool IsValidModifierType()
    {
        return ((type == MultiplicativeTypeEnum.attacker && Turn.unit == host)
            || (type == MultiplicativeTypeEnum.defender && Turn.targets.Contains(host.tile)));

    }
}
