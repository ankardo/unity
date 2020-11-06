using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public Unit unitPrefab;
    public List<Alliance> alliances;
    public List<Job> jobs;
    private Dictionary<string, Job> searchJobs;
    public List<UnitSerialized> serializedUnits;
    public static MapLoader instance;

    private GameObject holder;
    private void Awake()
    {
        instance = this;
        holder = new GameObject("Units Holder");
        BuildJobsDictionary();
        InitializeAlliances();
    }
    private void Start()
    {
        holder.transform.parent = Board.instance.transform;
    }
    public void CreateUnits()
    {
        for (int i = 0; i < serializedUnits.Count; i++)
        {
            CreateUnit(serializedUnits[i]);
        }
    }
    private void CreateUnit(UnitSerialized serialized)
    {
        CreateUnit(serialized.position, serialized.characterName, serialized.job, serialized.items,
            serialized.level, serialized.faction, serialized.playerType);
    }
    private void CreateUnit(Vector3Int pos, string name, string jobName, List<Item> items, int level, int faction, PlayerTypeEnum playerType)
    {
        LogicTile tile = Board.GetTile(pos);
        Unit unit = Instantiate(unitPrefab, tile.worldPos, Quaternion.identity, holder.transform);
        

        unit.tile = tile;
        unit.name = name;
        unit.faction = faction;
        unit.experience = unit.GetExpCurveValue(level);
        unit.playerType = playerType;
        Job job = searchJobs[jobName];
        Job.Employ(unit, job, level);
        CreateItems(items, unit);

        tile.content = unit.gameObject;
        Transform jumper = unit.transform.Find("Jumper");
        jumper.GetComponentInChildren<SpriteRenderer>().sortingOrder = unit.tile.contentOrder;

        StateMachineController.instance.units.Add(unit);
    }
    private void BuildJobsDictionary()
    {
        searchJobs = new Dictionary<string, Job>();
        foreach (Job job in jobs)
        {
            searchJobs.Add(job.name, job);
        }
    }
    private void InitializeAlliances()
    {
        for (int i = 0; i < alliances.Count; i++)
        {
            alliances[i].units = new List<Unit>();
            alliances[i].active = true;
        }
    }
    private void CreateItems(List<Item> items, Unit unit)
    {
        Transform itemHolder = unit.transform.Find("Equipment");
        for (int i = 0; i < items.Count; i++)
        {
            CreateItem(items[i], unit, itemHolder);
        }
    }
    private void CreateItem(Item item, Unit unit, Transform itemHolder)
    {
        Item instance = Instantiate(item, unit.transform.position, Quaternion.identity, itemHolder);
        instance.name = item.name;
        unit.equipment.Equip(instance);
    }
}
