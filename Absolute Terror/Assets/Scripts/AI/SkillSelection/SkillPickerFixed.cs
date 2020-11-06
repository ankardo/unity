using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPickerFixed : SkillPicker
{
    public string skillName;
    public override void Pick(AIPlan plan)
    {
        plan.skill = Find(skillName);
        plan.targetType = plan.skill.GetComponentInChildren<SkillAffects>().whoItAffects;
    }
}
