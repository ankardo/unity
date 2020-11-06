using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(AddUnitChargeTime());

    }

    public override void Exit()
    {
        base.Exit();
    }
    private IEnumerator AddUnitChargeTime()
    {
        if (Turn.hasActed)
            Turn.unit.chargeTime += Turn.ActionChargeTime;

        if (Turn.hasMoved)
            Turn.unit.chargeTime += Turn.MoveChargeTime;

        Turn.unit.chargeTime += Turn.EndTurnChargeTime;
        Turn.unit.chargeTime -= Turn.unit.GetStat(StatEnum.SPEED);
        Turn.hasActed = Turn.hasMoved = false;
        Turn.chosenSkill = null;
        Turn.item = null;
        ComputerPlayer.instance.currentPlan = null;
        stMachine.units.Remove(Turn.unit);
        stMachine.units.Add(Turn.unit);
        yield return null;
        CombatLog.CheckIfActive();
        yield return new WaitForSeconds(1);
        if (CombatLog.CheckIfCombatIsOver())
            stMachine.ChangeTo<CombatEndState>();
        else
            stMachine.ChangeTo<TurnBeginState>();
        

    }
}
