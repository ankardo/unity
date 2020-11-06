using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;

public enum DamageTypeEnum
{
    Physical,
    Magical,
}
public class SkillEffectDamage : SkillEffect
{
    public DamageTypeEnum damageType;
    public ElementalTypeEnum elementalType;
    public WeaponTypeEnum weaponType;
    public ArmorTypeEnum targetArmorType;


    [Header("Not in %")]
    public float baseMultiplier = 1;
    public float randomness = 0.2f;
    public float hitDelay = 0.1f;

    public override int Predict(Unit target)
    {
        float attackerScore = 0;
        float defenderScore = 0;
        switch (damageType)
        {
            case DamageTypeEnum.Physical:
                attackerScore += Turn.unit.GetStat(StatEnum.ATK);
                defenderScore += target.GetStat(StatEnum.DEF);
                weaponType = GetWeaponType(Turn.unit);
                targetArmorType = GetArmorType(target);
                break;
            case DamageTypeEnum.Magical:
                attackerScore += Turn.unit.GetStat(StatEnum.MATK);
                defenderScore += target.GetStat(StatEnum.MDEF);
                break;
        }

        float attackerFinalScore = GetBonus(Turn.unit, target, attackerScore);
        float defenderFinalScore = GetBonus(target, Turn.unit, defenderScore);
        float calculation = (attackerFinalScore - (defenderFinalScore / 2)) * baseMultiplier;
        calculation = Mathf.Clamp(calculation, 0, calculation);
        return (int)calculation;
    }
    public override void Apply(Unit target)
    {
        int damage = Predict(target);
        int currentHP = target.GetStat(StatEnum.HP);
        float roll = Random.Range(1 - randomness, 1 + randomness);
        int finalDamage = (int)(damage * roll);
        target.SetStat(StatEnum.HP, -finalDamage);
        CombatLog.log.Add(string.Format("{0} estava com {1} de HP, foi afetado por {2} e ficou com {3}",
            target, currentHP, finalDamage, target.GetStat(StatEnum.HP)));
        target.GotHurt(hitDelay);

    }
    private float GetBonus(Unit unit, Unit target, float initalScore)
    {
        if (unit.stats.multiplicativeModifiers == null)
            return initalScore;

        MultiplicativeForms forms = new MultiplicativeForms();
        forms.originalValue = (int)initalScore;
        forms.currentUnit = unit;
        forms.target = target;
        forms.elementalType = elementalType;
        forms.weaponType = weaponType;
        forms.targetArmorType = targetArmorType;
        unit.stats.multiplicativeModifiers(forms);
        float bonus = forms.currentValue;
        float scoreWithBonus = initalScore * (1 + (bonus / 100));
        Debug.LogFormat("Unit Checking: {3} Damage: {0} * bonus: {1}, score with bonus = {2}", initalScore, bonus, scoreWithBonus, unit);
        return scoreWithBonus;
    }
    private WeaponTypeEnum GetWeaponType(Unit unit)
    {
        Item item = unit.equipment.GetItem(ItemSlotEnum.MainHand);
        if (item == null)
            return WeaponTypeEnum.none;
        ModifierWeapon modifierWeapon = item.GetComponent<ModifierWeapon>();
        if (modifierWeapon != null)
            return modifierWeapon.type;
        return WeaponTypeEnum.none;
    }
    private ArmorTypeEnum GetArmorType(Unit unit)
    {
        Item item = unit.equipment.GetItem(ItemSlotEnum.UpperBody);
        if (item == null)
            return ArmorTypeEnum.none;
        ModifierArmor modifierArmor = item.GetComponent<ModifierArmor>();
        if (modifierArmor != null)
            return modifierArmor.type;
        return ArmorTypeEnum.none;
    }
    
}
