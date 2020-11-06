using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierConditionWeapon : ModifierCondition
{
    public WeaponTypeEnum weaponType;
    public ArmorTypeEnum armorType;
    public override bool Validate(object arg1)
    {
        MultiplicativeForms forms = (MultiplicativeForms)arg1;
        return (forms.weaponType == weaponType && forms.targetArmorType == armorType);
    }
}
