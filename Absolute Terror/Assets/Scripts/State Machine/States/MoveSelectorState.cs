using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectorState : State
{
    List<LogicTile> tiles;
    public override void Enter()
    {
        base.Enter();
        inputs.boardAxes = true;
        tiles = Board.instance.Search(Turn.unit.tile, Turn.unit.GetComponent<Movement>().CanMoveFromTo);
        Board.instance.HighlightTiles(tiles, Turn.unit.alliance);
        if (Turn.unit.playerType == PlayerTypeEnum.Human)
        {
            inputs.OnMove += OnMoveTileSelector;
            inputs.OnFire += OnFire;

            stMachine.mobileControlPanels[0].MoveTo("Show");
            stMachine.mobileControlPanels[1].MoveTo("Show");
        }
        else
            StartCoroutine(ComputerSelectMoveTarget());
        
    }

    public override void Exit()
    {
        base.Exit();
        inputs.boardAxes = false;
        if (Turn.unit.playerType == PlayerTypeEnum.Human)
        {
            inputs.OnMove -= OnMoveTileSelector;
            inputs.OnFire -= OnFire;
            stMachine.mobileControlPanels[0].MoveTo("Hide");
            stMachine.mobileControlPanels[1].MoveTo("Hide");
        }
        Board.instance.UndoHighlightTiles(tiles);
    }

    private void OnFire(object sender, object args)
    {
        int button = (int)args;
        if (button == 1 && tiles.Contains(stMachine.selectedTile))
            stMachine.ChangeTo<MoveSequenceState>();
        else if (button == 2)
            stMachine.ChangeTo<ChooseActionState>();
    }
    private IEnumerator ComputerSelectMoveTarget()
    {
        AIPlan plan = ComputerPlayer.instance.currentPlan;
        yield return StartCoroutine(AIMoveSelector(plan.movePos));
        yield return new WaitForSeconds(0.5f);
        stMachine.ChangeTo<MoveSequenceState>();
    }
}