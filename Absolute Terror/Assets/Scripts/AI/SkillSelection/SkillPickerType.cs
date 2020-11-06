using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPickerType : SkillPicker
{
    public SkillAffectsTypeEnum type;
    public override void Pick(AIPlan plan)
    {
        List<Skill> toPick = Find(type);
        plan.skill = toPick[Random.Range(0, toPick.Count)];
        plan.targetType = type;
    }
}
