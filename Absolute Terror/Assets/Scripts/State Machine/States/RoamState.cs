using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamState : State
{
    public override void Enter()
    {
        base.Enter();
        inputs.boardAxes = true;
        inputs.OnMove += OnMoveTileSelector;
        inputs.OnFire += OnFire;
        CheckNullPosition();
    }

    public override void Exit()
    {
        base.Exit();
        inputs.boardAxes = false;
        inputs.OnMove -= OnMoveTileSelector;
        inputs.OnFire -= OnFire;
    }
    private void OnFire(object sender, object args)
    {
        int button = (int)args;
        if (button == 2)
            stMachine.ChangeTo<ChooseActionState>();
    }
    private void CheckNullPosition()
    {
        if(Selector.instance.tile == null)
        {
            LogicTile tile = Board.GetTile(new Vector3Int(0,0,0));
            Selector.instance.tile = tile;
            Selector.instance.spriteRenderer.sortingOrder = tile.contentOrder;
            Selector.instance.transform.position = tile.worldPos;
        }
    }
}
