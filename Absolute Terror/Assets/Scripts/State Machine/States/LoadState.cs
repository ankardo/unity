using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(LoadSequence());
    }
    public IEnumerator LoadSequence()
    {
        yield return StartCoroutine(Board.instance.InitSequence(this));
        yield return null;
        yield return StartCoroutine(LoadAnimations());
        yield return null;
        MapLoader.instance.CreateUnits();
        yield return null;
        InitialTurnOrdering();
        yield return null;
        UnitAlliances();
        yield return null;
        List<Vector3Int> blockedTiles = Blockers.instance.GetBlockedTiles();
        yield return null;
        SetBlockedTiles(blockedTiles);
        yield return null;

        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }
    private void InitialTurnOrdering()
    {
        for (int i = 0; i < stMachine.units.Count; i++)
        {
            stMachine.units[i].chargeTime = Stat.MaxStat - stMachine.units[i].GetStat(StatEnum.SPEED);
            stMachine.units[i].active = true;
        }
    }
    private void UnitAlliances()
    {
        for (int i = 0; i < stMachine.units.Count; i++)
        {
            SetUnitAlliance(stMachine.units[i]);
        }
    }
    private void SetUnitAlliance(Unit unit)
    {
        for (int i = 0; i < MapLoader.instance.alliances.Count; i++)
        {
            if (MapLoader.instance.alliances[i].factions.Contains(unit.faction))
            {
                MapLoader.instance.alliances[i].units.Add(unit);
                unit.alliance = i;
                return;
            }
        }
    }
    private void SetBlockedTiles(List<Vector3Int> blockedTiles)
    {
        foreach (Vector3Int pos in blockedTiles)
        {
            LogicTile tile = Board.GetTile(pos);
            tile.content = Blockers.instance.gameObject;
        }
    }

    
    private IEnumerator LoadAnimations()
    {
        SpriteLoader[] loaders = SpriteLoader.holder.GetComponentsInChildren<SpriteLoader>();
        foreach (SpriteLoader loader in loaders)
        {
            yield return loader.Load();
        }
    }
}
