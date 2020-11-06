using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierConditionJob : ModifierCondition
{
    public string jobName;
    public override bool Validate(object arg1)
    {
        MultiplicativeForms forms = (MultiplicativeForms)arg1;
        return (forms.target.job.name == jobName);
    }
}
