using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public List<ItemSlots> itemSlots;
    public Item GetItem(ItemSlotEnum slot){
        return itemSlots[(int)slot].item;
    }
    private void Awake()
    {
        SetItemSlots();
    }
    private void SetItemSlots()
    {
        itemSlots = new List<ItemSlots>();
        for (int i = 0; i <= (int)ItemSlotEnum.Bag2; i++)
        {
            ItemSlots itemSlot = new ItemSlots();
            itemSlot.slot = (ItemSlotEnum)i;
            itemSlots.Add(itemSlot);
        }
    }
    public void Unequip(Item item)
    {
        foreach (ItemSlots slot in itemSlots)
        {
            if(slot.item == item)
            {
                slot.item = null;
                Destroy(item.gameObject);
            }
        }
    }
    public void Equip(Item item)
    {
        ItemSlots primary = itemSlots[(int)item.primarySlot];
        ItemSlots secondary = null;
        if (item.secondarySlot != ItemSlotEnum.None)
            secondary = itemSlots[(int)item.secondarySlot];
        if (item.useBoth)
        {
            if (primary.item == null && secondary.item == null)
            {
                primary.item = item;
                secondary.item = item;
                ActivateEquipabble(item);
            }
        }
        else
        {
            if (primary.item == null)
            {
                primary.item = item;
                ActivateEquipabble(item);
            }
            else
            {
                if (item.secondarySlot != ItemSlotEnum.None && secondary.item == null)
                {
                    secondary.item = item;
                    ActivateEquipabble(item);
                }
            }
        }
    }
    private void ActivateEquipabble(Item item)
    {
        Equippable equippable = item.GetComponent<Equippable>();
        if (equippable != null)
        {
            Unit unit = GetComponentInParent<Unit>();
            equippable.Use(unit);
        }
    }

}
