using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemSlotEnum primarySlot;
    public ItemSlotEnum secondarySlot;
    public bool useBoth;
    public Sprite icon;
    public abstract void Use(Unit unit);
}
