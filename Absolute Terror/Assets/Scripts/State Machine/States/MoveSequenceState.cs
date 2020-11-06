using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(MoveSequence());
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator MoveSequence()
    {
        List<LogicTile> path = CreatePath();

        Movement movement = Turn.unit.GetComponent<Movement>();
        yield return StartCoroutine(movement.Move(path));
        Turn.unit.tile.content = null;
        Turn.unit.tile = stMachine.selectedTile;
        Turn.unit.tile.content = Turn.unit.gameObject;
        yield return new WaitForSeconds(0.2f);
        Turn.hasMoved = true;

        stMachine.ChangeTo<ChooseActionState>();
    }

    private List<LogicTile> CreatePath()
    {
        List<LogicTile> path = new List<LogicTile>();

        LogicTile to = stMachine.selectedTile;

        while(Turn.unit.tile != to)
        {
            path.Add(to);
            to = to.prev;
        }
        path.Reverse();
        return path;
    }
}
