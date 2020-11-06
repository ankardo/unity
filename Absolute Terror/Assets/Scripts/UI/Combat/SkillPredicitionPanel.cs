using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPredicitionPanel : MonoBehaviour
{
    [HideInInspector]
    public Text skillName;
    [HideInInspector]
    public Text hitChance;
    [HideInInspector]
    public Text effect;
    [HideInInspector]
    public PanelPositioner panelPositioner;
    private void Awake()
    {
        panelPositioner = GetComponent<PanelPositioner>();
        skillName = transform.Find("SkillName").GetComponent<Text>();
        hitChance = transform.Find("HitChance").GetComponent<Text>();
        effect = transform.Find("Effect").GetComponent<Text>();
    }
    public void SetPredictionText()
    {
        int hitPrediction = 0;
        int effectPrediction = 0;
        Unit target = null;
        foreach (LogicTile tile in Turn.targets)
        {
            if(tile.content != null)
                target = tile.content.GetComponent<Unit>();
            if (target != null)
                break;
        }
        if (target != null)
        {
            hitPrediction = Turn.chosenSkill.GetHitPrediction(target);
            effectPrediction = Turn.chosenSkill.GetEffectPrediction(target);
            StateMachineController.instance.rightCharacterPanel.Show(target, false);
        }

        skillName.text = Turn.chosenSkill.name;
        hitChance.text = Mathf.Clamp(hitPrediction, 0, 100) + "% chance to Hit";
        effect.text = effectPrediction + " hitpoints";
    }
}

