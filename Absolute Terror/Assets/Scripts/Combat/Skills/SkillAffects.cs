using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillAffectsTypeEnum
{
    Default,
    Ally,
    Enemy,
}
public class SkillAffects : MonoBehaviour
{
    public SkillAffectsTypeEnum whoItAffects;
    public bool IsTarget(Unit unit)
    {
        switch (whoItAffects)
        {
            case SkillAffectsTypeEnum.Default:
                return true;
            case SkillAffectsTypeEnum.Ally:
                return SameAlliance(unit);
            case SkillAffectsTypeEnum.Enemy:
                return !SameAlliance(unit);
            default:
                return false;
        }
    }
    private bool SameAlliance(Unit unit){
        return (unit.alliance == Turn.unit.alliance);
    }
}
