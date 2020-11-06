using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillPicker : MonoBehaviour
{
    public const string defaultSkill = "NormalAttack";
    protected List<Skill> skills { get { return Turn.unit.job.skills; } }
    public abstract void Pick(AIPlan plan);
    protected List<Skill> Find(SkillAffectsTypeEnum type)
    {
        List<Skill> possibleSkills = new List<Skill>();
        foreach (Skill skill in skills)
        {
            if (skill.GetComponentInChildren<SkillAffects>().whoItAffects == type)
            {
                Debug.Log("Skill é do tipo certo");
                possibleSkills.Add(skill);
            }
        }
        if (possibleSkills.Count == 0)
            possibleSkills.Add(Default());
        return possibleSkills;
    }
    protected Skill Find(string skillName)
    {
        Skill chosenSkill = skills.Find(skill => skill.name == skillName);
        if (chosenSkill == null)
            return Default();
        
        return chosenSkill;
    }
    protected Skill Default()
    {
        return skills.Find(skill => skill.name == defaultSkill);
    }
}
