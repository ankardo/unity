using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSerialized 
{
    public string characterName;
    public string job;
    public List<Item> items;
    public Vector3Int position;
    public PlayerTypeEnum playerType;
    public int faction;
    public int level;
}
