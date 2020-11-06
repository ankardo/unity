using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOption
{
    class Hit
    {
        public LogicTile tile;
        public bool hasTarget;
        public Hit(LogicTile tile, bool hasTarget)
        {
            this.tile = tile;
            this.hasTarget = hasTarget;
        }
    }
    public LogicTile targetTile;
    public char direction;
    public List<LogicTile> areaTargets = new List<LogicTile>();
    public bool isCasterMatch;
    public LogicTile bestMoveTile;
    List<Hit> hits = new List<Hit>();
    public List<LogicTile> moveTargets = new List<LogicTile>();
    public void AddHit(LogicTile tile, bool hasTarget)
    {
        hits.Add(new Hit(tile, hasTarget));
    }
    public int GetScore(Unit caster, Skill skill)
    {
        GetBestMoveTarget(caster, skill);
        if (bestMoveTile == null)
            return 0;
        int score = 0;
        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i].hasTarget)
                score++;
            else
                score--;

        }
        if (isCasterMatch && areaTargets.Contains(bestMoveTile))
            score++;
        return score;
    }
    private void GetBestMoveTarget(Unit caster, Skill skill)
    {
        if (moveTargets.Count == 0)
            return;
        bestMoveTile = moveTargets[Random.Range(0, moveTargets.Count)];
    }
}
