using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetingState : State
{
    List<LogicTile> selectedTiles;
    bool directionOriented;
    public override void Enter()
    {
        base.Enter();

        selectedTiles = Turn.chosenSkill.GetTargets();
        board.HighlightTiles(selectedTiles, Turn.unit.alliance);
        

        directionOriented = Turn.chosenSkill.GetComponentInChildren<SkillRange>().IsDirectionOriented();
        if (Turn.unit.playerType == PlayerTypeEnum.Human)
        {
            if (directionOriented)
                inputs.OnMove += ChangeDirection;
            else
                inputs.OnMove += OnMoveTileSelector;

            inputs.OnFire += OnFire;
            stMachine.mobileControlPanels[0].MoveTo("Show");
            stMachine.mobileControlPanels[1].MoveTo("Show");
            stMachine.leftCharacterPanel.Show(Turn.unit, true);
            inputs.boardAxes = true;
        }
        else
            StartCoroutine(ComputerTargeting());
    }

    public override void Exit()
    {
        base.Exit();
        if (directionOriented)
            inputs.OnMove -= ChangeDirection;
        else
            inputs.OnMove -= OnMoveTileSelector;

        inputs.OnFire -= OnFire;

        stMachine.mobileControlPanels[0].MoveTo("Hide");
        stMachine.mobileControlPanels[1].MoveTo("Hide");
        inputs.boardAxes = false;
        board.UndoHighlightTiles(selectedTiles);
    }
    private void OnFire(object sender, object args)
    {
        int button = (int)args;
        if (button == 1 && (directionOriented || selectedTiles.Contains(Selector.instance.tile)))
        {
            Turn.targets = selectedTiles;
            stMachine.ChangeTo<SkillConfirmState>();
        }
        else
        {
            if (button == 2)
            {

                if (Turn.item != null)
                    stMachine.ChangeTo<ChooseItemState>();
                else
                    stMachine.ChangeTo<ChooseSkillState>();
            }
        }
    }
    private void ChangeDirection(object sender, object args)
    {
        Vector3Int pos = (Vector3Int)args;
        char dir = Turn.unit.tile.GetDirection(Turn.unit.tile.pos + pos);

        if (Turn.unit.direction != dir)
        {
            board.UndoHighlightTiles(selectedTiles);
            Turn.unit.direction = dir;
            Turn.unit.GetComponent<AnimationController>().Idle();
            selectedTiles = Turn.chosenSkill.GetTargets();
            board.HighlightTiles(selectedTiles, Turn.unit.alliance);
        }

    }
    private IEnumerator ComputerTargeting()
    {
        SkillRange range = Turn.chosenSkill.GetComponentInChildren<SkillRange>();
        AIPlan plan = ComputerPlayer.instance.currentPlan;
        if (directionOriented)
        {
            switch (plan.direction)
            {
                case 'N':
                    ChangeDirection(null, Vector3Int.up);
                    break;
                case 'S':
                    ChangeDirection(null, Vector3Int.down);
                    break;
                case 'E':
                    ChangeDirection(null, Vector3Int.right);
                    break;
                case 'W':
                    ChangeDirection(null, Vector3Int.left);
                    break;
            }
            yield return new WaitForSeconds(0.25f);
        }
        else
            yield return StartCoroutine(AIMoveSelector(plan.skillTargetPos));
        yield return new WaitForSeconds(1);
        Turn.targets = selectedTiles;
        stMachine.ChangeTo<SkillConfirmState>();
    }
}
