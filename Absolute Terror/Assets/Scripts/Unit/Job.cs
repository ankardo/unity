using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : ScriptableObject
{
    public List<Stat> stats;
    public List<Skill> skills;
    public GameObject AISkillPicks;
    public string spriteModel;
    [TextArea]
    public string description;
    public Sprite portrait;
    public int advancesAtLevel;
    public List<Job> advancesTo;
    public void InitStats()
    {
        stats = new List<Stat>();
        for (int i = 0; i <= (int)StatEnum.MOV; i++)
        {
            Stat temp = new Stat
            {
                type = (StatEnum)i,
                baseValue = 10,
                currentValue = 10,
                growth = 1
            };
            stats.Add(temp);
        }
    }
    public static bool CanAdvance(Unit unit)
    {
        return (unit.level >= unit.job.advancesAtLevel);
    }
    public static void Employ(Unit unit, Job job, int level)
    {
        unit.job = job;
        unit.spriteModel = job.spriteModel;
        unit.portrait = job.portrait;
        SetStats(unit.stats, job);
        unit.UpdateStats();
        unit.level = 0;
        unit.LevelUp(level);
        
        SkillBook skillBook = unit.GetComponentInChildren<SkillBook>();
        skillBook.skills = new List<Skill>();
        skillBook.skills.AddRange(job.skills);
    }

    private static void SetStats(Stats stats, Job job)
    {
        stats.overall = new List<Stat>();

        for (int i = 0; i < job.stats.Count; i++)
        {
            Stat stat = new Stat();
            stat.baseValue = job.stats[i].baseValue;
            stat.currentValue = job.stats[i].currentValue;
            stat.growth = job.stats[i].growth;
            stat.type = job.stats[i].type;
            stats.overall.Add(stat);
        }
        stats[StatEnum.HP].baseValue = stats[StatEnum.MaxHP].baseValue;
        stats[StatEnum.MP].baseValue = stats[StatEnum.MaxMP].baseValue;
    }
    private void GenerateRandomStats(ref Unit unit)
    {
        for (int i = 0; i < unit.stats.overall.Count; i++)
        {
            if (unit.stats.overall[i].type != StatEnum.MOV)
                unit.stats.overall[i].baseValue = Random.Range(1, 100);
            else
                unit.stats.overall[i].baseValue = Random.Range(1, 6);
        }
        unit.stats[StatEnum.HP].baseValue = unit.stats[StatEnum.MaxHP].baseValue;
        unit.stats[StatEnum.MP].baseValue = unit.stats[StatEnum.MaxMP].baseValue;

        unit.UpdateStats();
    }

}
