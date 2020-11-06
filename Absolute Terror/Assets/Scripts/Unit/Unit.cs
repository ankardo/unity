using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public delegate void OnTurnBegin();
public class Unit : MonoBehaviour
{
    [HideInInspector]
    public Stats stats;
    public Job job;
    public int level;
    [HideInInspector]
    public int experience;

    [HideInInspector]
    public int alliance;
    public Color allianceColor;
    [HideInInspector]
    public int faction;
    [HideInInspector]
    public int chargeTime;
    [HideInInspector]
    public LogicTile tile;
    private bool _active;
    public bool active
    {
        get { return _active; }
        set
        {
            if (_active && !value)
                GiveExp();
            _active = value;
        }
    }
    [HideInInspector]
    public string spriteModel;
    [HideInInspector]
    public char direction = 'S';
    [HideInInspector]
    public AnimationController animationController;
    public SpriteSwapper spriteSwapper;
    public OnTurnBegin OnTurnBegin;
    [HideInInspector]
    public Sprite portrait;
    public Image healthBar;
    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip gotHit;
    public AudioClip died;
    public Equipment equipment;
    public PlayerTypeEnum playerType;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animationController = GetComponent<AnimationController>();
        stats = GetComponentInChildren<Stats>();
        spriteSwapper = transform.Find("Jumper/UnitSprite").GetComponent<SpriteSwapper>();
        healthBar.color = Color.green;
        equipment = GetComponentInChildren<Equipment>();
    }
    private void Start()
    {
        spriteSwapper.unitSprites = SpriteLoader.holder.Find(spriteModel).GetComponent<SpriteLoader>();
        animationController.Idle();
    }
    public int GetStat(StatEnum statName)
    {
        return stats[statName].currentValue;
    }
    public void SetStat(StatEnum statName, int statChange)
    {
        if (statName == StatEnum.HP)
        {
            stats[statName].currentValue = ClampStat(StatEnum.MaxHP, this.GetStat(statName) + statChange);
            SetStatBar(statName, StatEnum.MaxHP);
            PopCombatText(statName, statChange);

        }
        else if (statName == StatEnum.MP)
        {
            stats[statName].currentValue = ClampStat(StatEnum.MaxMP, this.GetStat(statName) + statChange);
            SetStatBar(statName, StatEnum.MaxMP);
            PopCombatText(statName, statChange);
        }
    }

    private void SetStatBar(StatEnum statName, StatEnum maxValueStatName)
    {
        float maxValue = (float)GetStat(maxValueStatName);
        float fillValue = (GetStat(statName) * 100 / maxValue) / 100;
        healthBar.fillAmount = fillValue;

        if (statName == StatEnum.HP)
        {
            if (fillValue >= 0.7f)
                healthBar.color = Color.green;
            else if (fillValue >= 0.3f)
                healthBar.color = Color.yellow;
            else
                healthBar.color = Color.red;
        }


    }
    private int ClampStat(StatEnum type, int value)
    {
        return Mathf.Clamp(value, 0, stats[type].currentValue);
    }
    public void UpdateStat(StatEnum statName)
    {
        Stat toUpdate = stats[statName];
        toUpdate.currentValue = stats[statName].baseValue;

        if (toUpdate.statModifiers != null)
            toUpdate.statModifiers(toUpdate);
    }
    public void UpdateStats()
    {
        foreach (Stat stat in stats.overall)
        {
            UpdateStat(stat.type);
        }
    }
    public void GotHurt(float hitDelay)
    {
        if (stats[StatEnum.HP].currentValue <= 0)
        {

            if (died != null)
            {
                audioSource.clip = died;
                audioSource.PlayDelayed(hitDelay);
            }
            animationController.Death(hitDelay);
        }
        else
        {
            animationController.Idle();
            if (gotHit != null)
            {
                audioSource.clip = gotHit;
                audioSource.PlayDelayed(hitDelay);
            }
            animationController.GotHit(hitDelay);
        }
    }
    private void PopCombatText(StatEnum statName, int value)
    {
        CombatText.instance.PopText(this, value, statName);
    }
    public void LevelUp(int amount)
    {
        Stats toLevelStats = stats;
        foreach (Stat stat in toLevelStats.overall)
        {
            stat.baseValue += Mathf.FloorToInt(stat.growth * amount);
        }
        level += amount;
        experience = 0;
        toLevelStats[StatEnum.HP].baseValue = toLevelStats[StatEnum.MaxHP].baseValue;
        toLevelStats[StatEnum.MP].baseValue = toLevelStats[StatEnum.MaxMP].baseValue;
        UpdateStats();
        SetStatBar(StatEnum.HP, StatEnum.MaxHP);
        SetStatBar(StatEnum.MP, StatEnum.MaxMP);
    }
    public int GetExpCurveValue(int toLevel)
    {
        int value = 0;
        for (int i = 1; i < toLevel; i++)
        {
            value += i * 100;
        }
        return value;
    }
    public void CheckLevelUp()
    {
        int required = GetExpCurveValue(level + 1);
        if (experience >= required)
            LevelUp(1);
        
    }
    private void GiveExp()
    {
        foreach (Unit unit in StateMachineController.instance.units)
        {
            if (unit.active && unit.alliance != alliance)
            {
                unit.experience += 100;
                unit.CheckLevelUp();
            }
        }
    }
}
