using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseItemState : State
{

    List<Consumable> consumables;
    public override void Enter()
    {
        base.Enter();
        index = 0;
        inputs.OnMove += OnMove;
        inputs.OnFire += OnFire;
        currentUISelector = stMachine.chooseSkillSelector;

        CheckItems();
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
        stMachine.mobileControlPanels[0].MoveTo("Hide");
        stMachine.mobileControlPanels[1].MoveTo("Hide");
        stMachine.chooseSkillPanel.MoveTo("Hide");
    }

    private void CheckItems()
    {
        consumables = new List<Consumable>();
        int bag = (int)ItemSlotEnum.Bag1;
        for (int j = 0; j < 2; j++, bag++)
        {
            Item item = Turn.unit.equipment.GetItem((ItemSlotEnum)bag);
            if (item != null)
                consumables.Add(item.GetComponent<Consumable>());

        }
        for (int i = 0; i < 5; i++)
        {
            if (i < consumables.Count)
            {
                if (consumables[i].icon == null)
                    stMachine.chooseSkillButtons[i].sprite = consumables[i].skill.icon;
                else
                    stMachine.chooseSkillButtons[i].sprite = consumables[i].icon;
            }
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
    private void ButtonActions()
    {
        if (index >= consumables.Count)
            return;
        if (consumables[index].skill.CanUse())
        {
            Turn.chosenSkill = consumables[index].skill;
            Turn.item = consumables[index];
            stMachine.ChangeTo<SkillTargetingState>();
        }
    }
    private void OnFire(object sender, object args)
    {
        int button = (int)args;
        if (button == 1)
            ButtonActions();
        else if (button == 2)
        {
            stMachine.leftCharacterPanel.Hide();
            Turn.item = null;
            stMachine.ChangeTo<ChooseActionState>();
        }
    }

}
