using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseActionState : State
{
    public override void Enter()
    {

        base.Enter();
        MoveSelector(Turn.unit.tile);
        if (Turn.unit.playerType == PlayerTypeEnum.Human)
        {

            index = 0;
            inputs.OnMove += OnMove;
            inputs.OnFire += OnFire;
            currentUISelector = stMachine.chooseActionSelector;

            CheckActionsState();
            MoveUISelectorToCurrentIndex(stMachine.chooseActionButtons);

            stMachine.chooseActionPanel.MoveTo("Show");
            inputs.OnButtonClicked += ButtonActions;
        }
        else
            StartCoroutine(ComputerChooseAction());
    }

    public override void Exit()
    {
        base.Exit();
        inputs.OnMove -= OnMove;
        inputs.OnFire -= OnFire;
        inputs.OnButtonClicked -= ButtonActions;
        stMachine.chooseActionPanel.MoveTo("Hide");
    }
    private void LateUpdate()
    {
        if (Turn.hasActed && Turn.hasMoved)
            stMachine.ChangeTo<TurnEndState>();
    }
    private void OnMove(object sender, object args)
    {
        Vector3Int button = (Vector3Int)args;
        if (button == Vector3Int.left)
        {
            index--;
            MoveUISelectorToCurrentIndex(stMachine.chooseActionButtons);
        }
        else if (button == Vector3Int.right)
        {
            index++;
            MoveUISelectorToCurrentIndex(stMachine.chooseActionButtons);
        }
    }
    private void OnFire(object sender, object args)
    {
        int button = (int)args;
        if (button == 1)
            ButtonActions(index);
        else if (button == 2)
            stMachine.ChangeTo<RoamState>();

    }
    private void ButtonActions(int index)
    {
        switch (index)
        {
            case 0:
                if (!Turn.hasMoved)
                    stMachine.ChangeTo<MoveSelectorState>();
                break;
            case 1:
                if (!Turn.hasActed)
                    stMachine.ChangeTo<ChooseSkillState>();
                break;
            case 2:
                stMachine.ChangeTo<ChooseItemState>();
                break;
            case 3:
                stMachine.ChangeTo<TurnEndState>();
                break;

        }
    }
    private void CheckActionsState()
    {
        SetButtonStateColor(stMachine.chooseActionButtons[0], Turn.hasMoved);
        SetButtonStateColor(stMachine.chooseActionButtons[1], Turn.hasActed);
        SetButtonStateColor(stMachine.chooseActionButtons[2], Turn.hasActed);
    }
    private IEnumerator ComputerChooseAction()
    {
        AIPlan plan = ComputerPlayer.instance.currentPlan;
        if (plan == null)
        {
            plan = ComputerPlayer.instance.Evaluate();
            Turn.chosenSkill = plan.skill;
        }
        yield return new WaitForSeconds(1);
        if (Turn.hasMoved == false && plan.movePos != Turn.unit.tile.pos)
        {
            stMachine.ChangeTo<MoveSelectorState>();
        }
        else if (Turn.hasActed == false && Turn.chosenSkill != null)
        {
            stMachine.ChangeTo<SkillTargetingState>();
        }
        else
        {
            stMachine.ChangeTo<TurnEndState>();
        }


    }
}
