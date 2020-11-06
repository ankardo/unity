using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfirmState : State
{
    public override void Enter()
    {
        base.Enter();
        
        Turn.targets = Turn.chosenSkill.GetArea();
        board.HighlightTiles(Turn.targets, Turn.unit.alliance);

        stMachine.skillPredicitionPanel.SetPredictionText();
        stMachine.skillPredicitionPanel.panelPositioner.MoveTo("Show");
        stMachine.leftCharacterPanel.Show(Turn.unit, false);

        if(Turn.unit.playerType == PlayerTypeEnum.Human){
            stMachine.mobileControlPanels[1].MoveTo("Show");
            inputs.OnFire += OnFire;
            stMachine.rightCharacterPanel.Show(Turn.unit, true);
        }
        else{
            stMachine.rightCharacterPanel.Show(Turn.unit, false);
            StartCoroutine(ComputerConfirmSkill());
        }
    }

    public override void Exit()
    {
        base.Exit();
        if(Turn.unit.playerType == PlayerTypeEnum.Human){
            stMachine.mobileControlPanels[1].MoveTo("Hide");
            inputs.OnFire -= OnFire;
        }
        stMachine.skillPredicitionPanel.panelPositioner.MoveTo("Hide");
        stMachine.rightCharacterPanel.Hide();
        board.UndoHighlightTiles(Turn.targets);
    }
    private void OnFire(object sender, object args)
    {
        int button = (int)args;
        if (button == 1 )
        {
            if(Turn.chosenSkill.hasValidTarget(Turn.targets)){
                stMachine.leftCharacterPanel.Hide();
                stMachine.ChangeTo<SkillPerformState>();
            }
            else 
                Debug.Log("Invalid Target");
        }
        else if (button == 2)
            stMachine.ChangeTo<SkillTargetingState>();
    }
    private IEnumerator ComputerConfirmSkill(){
        yield return new WaitForSeconds(1.5f);
        stMachine.leftCharacterPanel.Hide();
        stMachine.ChangeTo<SkillPerformState>();
    }
}
