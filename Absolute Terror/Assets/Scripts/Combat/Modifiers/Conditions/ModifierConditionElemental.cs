using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierConditionElemental : ModifierCondition
{
    public ElementalTypeEnum type;
    public override bool Validate(object arg1)
    {
        MultiplicativeForms forms = (MultiplicativeForms)arg1;
        if (forms.elementalType == ElementalTypeEnum.weapon)
        {
            Item item  = Turn.unit.equipment.GetItem(ItemSlotEnum.MainHand);
            if( item == null)
                return false;
            ModifierElemental modifierElemental = item.GetComponent<ModifierElemental>();
            return (modifierElemental != null && modifierElemental.type == type);
        }
        return (forms.elementalType == type);
    }
}
