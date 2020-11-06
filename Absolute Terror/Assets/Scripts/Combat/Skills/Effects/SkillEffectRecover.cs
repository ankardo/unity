using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RecoverTypeEnum
{
    Health,
    Mana,
}
public class SkillEffectRecover : SkillEffect
{
    public RecoverTypeEnum recoverType;
    [Header("Not in %")]
    public float baseMultiplier = 1;
    public float randomness = 0.2f;
    public float hitDelay = 0.1f;

    public override int Predict(Unit target)
    {
        float casterScore = 0;
        casterScore = Turn.unit.GetStat(StatEnum.VIR) * baseMultiplier;
        return (int)casterScore;
    }
    public override void Apply(Unit target)
    {
        int initial = Predict(target);
        float roll = Random.Range(1 - randomness, 1 + randomness);
        int beforeRecovery = 0;
        int recovery = (int)(initial * roll);

        switch (recoverType)
        {
            case RecoverTypeEnum.Health:
                beforeRecovery = target.GetStat(StatEnum.HP);
                target.SetStat(StatEnum.HP, recovery);
                CombatLog.log.Add(string.Format("{0} estava com {1} de HP, foi curado por {2} e ficou com {3}",
                    target, beforeRecovery, recovery, target.GetStat(StatEnum.HP)));
                break;
            case RecoverTypeEnum.Mana:
                beforeRecovery = target.GetStat(StatEnum.MP);
                target.SetStat(StatEnum.MP, recovery);
                CombatLog.log.Add(string.Format("{0} estava com {1} de MP, recuperou {2} de Mana e ficou com {3}",
                    target, beforeRecovery, recovery, target.GetStat(StatEnum.MP)));
                break;
        }

    }
}
