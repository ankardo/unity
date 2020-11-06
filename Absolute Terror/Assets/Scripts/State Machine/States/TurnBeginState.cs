using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBeginState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(SelectUnit());
    }

    public override void Exit()
    {
        base.Exit();
    }
    IEnumerator SelectUnit()
    {
        stMachine.units.Sort((x, y) => x.chargeTime.CompareTo(y.chargeTime));
        Turn.unit = stMachine.units[0];
        
        yield return null;
        if(Turn.unit.OnTurnBegin != null){
            int healthBeforeEffect = Turn.unit.GetStat(StatEnum.HP);
            Turn.unit.OnTurnBegin();
            if(healthBeforeEffect > Turn.unit.GetStat(StatEnum.HP))
                Turn.unit.GotHurt(0.2f);
        }
        yield return null;
        if(Turn.unit.GetStat(StatEnum.HP) <= 0 || Turn.unit.active == false)
            stMachine.ChangeTo<TurnEndState>();
        else
        {
            if(Job.CanAdvance(Turn.unit))
                stMachine.ChangeTo<ChooseJobAdvanceState>();
            else
                stMachine.ChangeTo<ChooseActionState>();
        }
        
    }
    private void untie()
    {
        for (int i = 0; i < stMachine.units.Count - 1; i++)
        {
            if (stMachine.units[i].chargeTime == stMachine.units[i + 1].chargeTime)
            {
                stMachine.units[i + 1].chargeTime++;
            }
        }
    }
}
