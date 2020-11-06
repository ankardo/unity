using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitChanceTypeEnum
{
    Damage,
    Debuff,
    Full
}
public class HitChance : MonoBehaviour
{
    public HitChanceTypeEnum type;
    public float baseBonusChance;
    public int baseChance = 75;
    public int Predict(Unit target)
    {
        float hitScore = 0;
        float missScore = 0;
        switch (type)
        {
            case HitChanceTypeEnum.Full:
                return 100;
            case HitChanceTypeEnum.Damage:
                hitScore = Turn.unit.GetStat(StatEnum.ACC);
                missScore = target.GetStat(StatEnum.EVD);
                break;
            case HitChanceTypeEnum.Debuff:
                hitScore = Turn.unit.GetStat(StatEnum.ACC);
                missScore = target.GetStat(StatEnum.RES);
                break;
        }
        float chance = baseChance - (missScore - hitScore);
        return (int)chance;
    }
    public bool TryToHit(Unit target)
    {
        float chance = Predict(target);
        float roll = Random.Range(0, 100 - baseBonusChance);
        chance += roll + baseBonusChance;
        Debug.LogFormat("Base Chance from stats is {0}, Rolled for {1} + {2} from bonusChance", chance, roll, baseBonusChance);
        return (chance >= 100);
    }
}
