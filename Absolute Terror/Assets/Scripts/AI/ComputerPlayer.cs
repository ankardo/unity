using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    public static ComputerPlayer instance;
    private Unit currentUnit { get { return Turn.unit; } }
    int alliance { get { return currentUnit.alliance; } }
    private Unit nearestFoe;
    public AIPlan currentPlan;
    private void Awake()
    {
        instance = this;
    }
    public AIPlan Evaluate()
    {
        AIPlan plan = new AIPlan();
        AISkillBehaviour aISkillBehaviour = Turn.unit.GetComponent<AISkillBehaviour>();
        if (aISkillBehaviour == null)
            aISkillBehaviour = Turn.unit.gameObject.AddComponent<AISkillBehaviour>();
        TryToEvaluate(plan, aISkillBehaviour);
        if (plan.skill == null)
            MoveTowardsOponent(plan);

        currentPlan = plan;
        return plan;
    }
    private void FindNearestFoe()
    {
        nearestFoe = null;
        Board.instance.Search(Turn.unit.tile, delegate (LogicTile arg1, LogicTile arg2)
        {
            if (nearestFoe == null && arg2.content != null)
            {
                Unit unit = arg2.content.GetComponent<Unit>();
                if (unit != null && currentUnit.alliance != unit.alliance)
                {
                    Stats stats = unit.stats;
                    if (stats[StatEnum.HP].currentValue > 0 && unit.active)
                    {
                        nearestFoe = unit;
                        return true;
                    }
                }
            }
            arg2.distance = arg1.distance + 1;
            return nearestFoe == null;
        });
    }
    private List<LogicTile> GetMoveOptions()
    {
        return Board.instance.Search(Turn.unit.tile, Turn.unit.GetComponent<Movement>().CanMoveFromTo);
    }
    private List<LogicTile> GetMoveOptions(bool includeCurrenPosition)
    {
        List<LogicTile> moveOptions = GetMoveOptions();
        moveOptions.Add(Turn.unit.tile);
        return moveOptions;
    }
    private bool IsDirectionIndependent(AIPlan plan)
    {
        SkillRange range = plan.skill.GetComponentInChildren<SkillRange>();
        return !range.IsDirectionOriented();

    }
    private bool TryToEvaluate(AIPlan plan, AISkillBehaviour aISkillBehaviour)
    {
        aISkillBehaviour.Pick(plan);
        if (plan.skill == null)
        {
            Debug.Log("Não Escolheu nenhuma skill");
            return false;
        }
        Debug.Log("Escolheu a skill: " + plan.skill);
        if (IsDirectionIndependent(plan))
        {
            Debug.Log("Nao depende de direcao");
            PlanDirectionIndependent(plan);
        }
        else
        {
            Debug.Log("Depende de direcao");
            PlanDirectionDependent(plan);
        }
        return true;
    }
    private void MoveTowardsOponent(AIPlan plan)
    {
        List<LogicTile> moveOptions = GetMoveOptions();
        FindNearestFoe();
        if (nearestFoe != null)
        {
            LogicTile toCheck = nearestFoe.tile;
            while (toCheck != null)
            {
                if (moveOptions.Contains(toCheck))
                {
                    plan.movePos = toCheck.pos;
                    return;
                }
                toCheck = toCheck.prev;
            }
        }
        plan.movePos = Turn.unit.tile.pos;
    }
    private void PlanDirectionIndependent(AIPlan plan)
    {
        LogicTile startTile = Turn.unit.tile;
        LogicTile selectorStartTile = Selector.instance.tile;
        Dictionary<LogicTile, AttackOption> map = new Dictionary<LogicTile, AttackOption>();
        SkillRange skillRange = plan.skill.GetComponentInChildren<SkillRange>();
        List<LogicTile> moveOptions = GetMoveOptions(true);

        for (int i = 0; i < moveOptions.Count; i++)
        {
            LogicTile destinyTile = moveOptions[i];
            Turn.unit.tile.content = null;
            Turn.unit.tile = destinyTile;
            destinyTile.content = Turn.unit.gameObject;
            List<LogicTile> fireOptions = skillRange.GetTilesInRange();
            for (int j = 0; j < fireOptions.Count; j++)
            {
                LogicTile fireTile = fireOptions[j];
                AttackOption attackOption = null;
                if (map.ContainsKey(fireTile))
                    attackOption = map[fireTile];
                else
                {
                    attackOption = new AttackOption();
                    map[fireTile] = attackOption;
                    attackOption.targetTile = fireTile;
                    attackOption.direction = Turn.unit.direction;
                    RateFireLocation(plan, attackOption);
                }
                attackOption.moveTargets.Add(destinyTile);
            }
        }
        Turn.unit.tile.content = null;
        Turn.unit.tile = startTile;
        startTile.content = Turn.unit.gameObject;
        Selector.instance.tile = selectorStartTile;
        List<AttackOption> attackOptions = new List<AttackOption>(map.Values);
        PickBestOption(plan, attackOptions);

    }
    private void PlanDirectionDependent(AIPlan plan)
    {
        LogicTile startTile = Turn.unit.tile;
        LogicTile selectorStartTile = Selector.instance.tile;
        char startDirection = Turn.unit.direction;
        List<AttackOption> attackOptions = new List<AttackOption>();
        List<LogicTile> moveOptions = GetMoveOptions(true);
        char[] directions = new char[] { 'N', 'S', 'E', 'W' };

        for (int i = 0; i < moveOptions.Count; i++)
        {
            LogicTile destinyTile = moveOptions[i];
            Turn.unit.tile.content = null;
            Turn.unit.tile = destinyTile;
            destinyTile.content = Turn.unit.gameObject;
            for (int j = 0; j < 4; j++)
            {
                Turn.unit.direction = directions[j];
                AttackOption attackOption = new AttackOption();
                attackOption.targetTile = destinyTile;
                attackOption.direction = Turn.unit.direction;
                RateFireLocation(plan, attackOption);
                attackOption.moveTargets.Add(destinyTile);
                attackOptions.Add(attackOption);
            }
        }
        Turn.unit.tile.content = null;
        Turn.unit.tile = startTile;
        startTile.content = Turn.unit.gameObject;
        Turn.unit.direction = startDirection;
        Selector.instance.tile = selectorStartTile;
        PickBestOption(plan, attackOptions);
    }

    private bool HasSkillTarget(AIPlan plan, LogicTile tile, Unit unit)
    {
        if (unit == null)
            return false;
        switch (plan.targetType)
        {
            case SkillAffectsTypeEnum.Default:
                return true;
            case SkillAffectsTypeEnum.Enemy:
                if (unit.active && unit.alliance != Turn.unit.alliance)
                    return true;
                break;
            case SkillAffectsTypeEnum.Ally:
                if (unit.active && unit.alliance == Turn.unit.alliance)
                    return true;
                break;
        }
        return false;
    }
    private void RateFireLocation(AIPlan plan, AttackOption attackOption)
    {
        List<LogicTile> tiles = plan.skill.GetTargets();
        AOE area = plan.skill.GetComponentInChildren<AOE>();
        Selector.instance.tile = attackOption.targetTile;

        tiles = area.GetArea(tiles);
        attackOption.areaTargets = tiles;
        attackOption.isCasterMatch = HasSkillTarget(plan, Turn.unit.tile, Turn.unit);

        for (int i = 0; i < tiles.Count; i++)
        {
            LogicTile currentTile = tiles[i];
            if (Turn.unit.tile == currentTile || currentTile.content == null)
                continue;
            Unit unit = currentTile.content.GetComponent<Unit>();
            if (unit == null || !unit.active)
                continue;

            bool hasTarget = HasSkillTarget(plan, currentTile, unit);
            attackOption.AddHit(currentTile, hasTarget);
        }
    }
    private void PickBestOption(AIPlan plan, List<AttackOption> attackOptions)
    {
        int bestScore = 1;
        List<AttackOption> bestOptions = new List<AttackOption>();
        for (int i = 0; i < attackOptions.Count; i++)
        {
            AttackOption attackOption = attackOptions[i];
            int score = attackOption.GetScore(Turn.unit, plan.skill);
            if (score > bestScore)
            {
                bestScore = score;
                bestOptions.Clear();
                bestOptions.Add(attackOption);
            }
            else
            {
                if (score == bestScore)
                    bestOptions.Add(attackOption);

            }
        }
        if (bestOptions.Count == 0)
        {
            plan.skill = null;
            return;
        }
        AttackOption choice = bestOptions[Random.Range(0, bestOptions.Count)];
        plan.skillTargetPos = choice.targetTile.pos;
        plan.direction = choice.direction;
        plan.movePos = choice.bestMoveTile.pos;
    }
}
