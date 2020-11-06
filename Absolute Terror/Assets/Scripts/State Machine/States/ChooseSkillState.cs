using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSkillState : State
{
    List<Skill> skills;
    public override void Enter()
    {
        base.Enter();
        index = 0;
        inputs.OnMove += OnMove;
        inputs.OnFire += OnFire;
        inputs.OnButtonClicked += ButtonActions;
        currentUISelector = stMachine.chooseSkillSelector;

        CheckSkills();
        MoveUISelectorToCurrentIndex(stMachine.chooseSkillButtons);

        stMachine.leftCharacterPanel.Show(Turn.unit, true);
        stMachine.mobileControlPanels[0].MoveTo("Show");
        stMachine.mobileControlPanels[1].MoveTo("Show");
        stMachine.chooseSkillPanel.MoveTo("Show");
        
    }

    public override void Exit()
    {
        base.Exit();
        inputs.OnMove -= OnMove;
        inputs.OnFire -= OnFire;
        inputs.OnButtonClicked -= ButtonActions;
        stMachine.mobileControlPanels[0].MoveTo("Hide");
        stMachine.mobileControlPanels[1].MoveTo("Hide");
        stMachine.chooseSkillPanel.MoveTo("Hide");
        
    }

    private void CheckSkills()
    {
        Transform skillBook = Turn.unit.transform.Find("SkillBook");
        skills = new List<Skill>();
        skills.AddRange(skillBook.GetComponent<SkillBook>().skills);
        for (int i = 0; i < 5; i++)
        {
            if (i < skills.Count)
                stMachine.chooseSkillButtons[i].sprite = skills[i].icon;
            else
                stMachine.chooseSkillButtons[i].sprite = stMachine.cantChooseSkill;
        }
    }
    private void OnMove(object sender, object args)
    {
        Vector3Int button = (Vector3Int)args;
        if (button == Vector3Int.up)
        {
            index--;
            MoveUISelectorToCurrentIndex(stMachine.chooseSkillButtons);
        }
        else if (button == Vector3Int.down)
        {
            index++;
            MoveUISelectorToCurrentIndex(stMachine.chooseSkillButtons);
        }
    }
    private void ButtonActions(int index)
    {
        if (index >= skills.Count)
            return;
        if (skills[index].CanUse())
        {
            Turn.chosenSkill = skills[index];
            stMachine.ChangeTo<SkillTargetingState>();
        }
    }
    private void OnFire(object sender, object args)
    {
        int button = (int)args;
        if (button == 1)
            ButtonActions(index);
        else if (button == 2){
            stMachine.leftCharacterPanel.Hide();
            stMachine.ChangeTo<ChooseActionState>();
        }
    }

    
}
