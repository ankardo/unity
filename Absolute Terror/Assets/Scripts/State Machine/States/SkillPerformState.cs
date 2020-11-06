using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPerformState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(PerformSequence());
    }

    IEnumerator PerformSequence()
    {
        yield return null;
        Turn.unit.direction = Turn.chosenSkill.GetComponentInChildren<SkillRange>().GetDirection();
        Turn.unit.animationController.Idle();
        Turn.unit.animationController.Attack(Turn.chosenSkill.animationName);
        Turn.chosenSkill.Perform();
        yield return new WaitForSeconds(1);
        if(Turn.item != null)
            Turn.unit.equipment.Unequip(Turn.item);   
        stMachine.ChangeTo<TurnEndState>();
    }
}
