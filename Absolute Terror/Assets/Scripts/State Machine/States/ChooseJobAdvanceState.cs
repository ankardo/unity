using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseJobAdvanceState : State
{
    public override void Enter()
    {
        base.Enter();
        stMachine.advanceJobPanel.Show();
        inputs.OnMove += OnMove;
        inputs.OnFire += OnFire;
    }
    public override void Exit()
    {
        base.Exit();
        stMachine.advanceJobPanel.Hide();
        inputs.OnMove -= OnMove;
        inputs.OnFire -= OnFire;
    }
    
    private void OnMove(object sender, object args)
    {
        Vector3Int input = (Vector3Int)args;
        if(input == Vector3Int.left)
            stMachine.advanceJobPanel.SelectPrevious();
        else if(input == Vector3Int.right)
            stMachine.advanceJobPanel.SelectNext();
    }
    private void OnFire(object sender, object args)
    {
        int input = (int)args;
        if(input == 1){
            stMachine.advanceJobPanel.JobChange();
            stMachine.ChangeTo<ChooseActionState>();
        }
    }
    
}
