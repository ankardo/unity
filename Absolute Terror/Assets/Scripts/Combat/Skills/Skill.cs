using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int manaCost;

    public Sprite icon;
    public string animationName;
    private Transform _primary;
    private Transform primary
    {
        get
        {
            if (_primary == null)
            {
                _primary = transform.Find("Primary");
                secondary = transform.Find("Secondary");
            }
            return _primary;
        }
    }
    private Transform secondary;
    public bool CanUse()
    {
        return (Turn.unit.GetStat(StatEnum.MP) >= manaCost || Turn.item != null);
    }

    public bool hasValidTarget(List<LogicTile> targets)
    {
        foreach (LogicTile tile in targets)
        {
            if (tile.content != null)
            {
                Unit target = tile.content.GetComponent<Unit>();

                if (target != null && GetComponentInChildren<SkillAffects>().IsTarget(target) && target.active)
                    return true;
            }
        }
        return false;
    }

    public List<LogicTile> GetTargets()
    {
        return GetComponentInChildren<SkillRange>().GetTilesInRange();
    }
    public List<LogicTile> GetArea()
    {
        return GetComponentInChildren<AOE>().GetArea(Turn.targets);
    }

    public void Perform()
    {
        Turn.targets.RemoveAll(target => target.content == null);

        for (int i = 0; i < Turn.targets.Count; i++)
        {
            Unit target = Turn.targets[i].content.GetComponent<Unit>();
            if (target != null && target.active){

                bool didHit = PassesAbilityCheck(target, primary);
                VFX(target, didHit);
                SFX(didHit);
                if (didHit)
                {
                    primary.GetComponentInChildren<SkillEffect>().Apply(target);
                    if (secondary.childCount > 0 && PassesAbilityCheck(target, secondary)){
                        secondary.GetComponentInChildren<SkillEffect>().Apply(target);
                    }
                    else
                    {
                        if (secondary.childCount > 0)
                            CombatText.instance.PopText(target, "Resist", Color.white);

                    }

                }
                else
                    CombatText.instance.PopText(target, "Miss", Color.white);
            }

            
        }
    }
    public int GetHitPrediction(Unit target)
    {
        return primary.GetComponentInChildren<HitChance>().Predict(target);
    }
    public int GetHitPrediction(Unit target, Transform effect)
    {
        return effect.GetComponentInChildren<HitChance>().Predict(target);
    }
    public int GetEffectPrediction(Unit target)
    {
        return primary.GetComponentInChildren<SkillEffect>().Predict(target);
    }
    public int GetEffectPrediction(Unit target, Transform effect)
    {
        return effect.GetComponentInChildren<SkillEffect>().Predict(target);
    }
    private bool PassesAbilityCheck(Unit target, Transform effect)
    {
        return effect.GetComponentInChildren<HitChance>().TryToHit(target);
    }
    private void VFX(Unit target, bool didHit){
        SkillVisualFX skillVisualFX = GetComponentInChildren<SkillVisualFX>();
        if(skillVisualFX != null)
        {
            skillVisualFX.target = target;
            skillVisualFX.didHit = didHit;
            skillVisualFX.VFX();
        }
    }
    private void SFX(bool didHit){
        SkillSoundFX skillSoundFX = GetComponentInChildren<SkillSoundFX>();
        if(skillSoundFX != null)
        {
            skillSoundFX.didHit = didHit;
            skillSoundFX.Play();
        }
    }
    
}
