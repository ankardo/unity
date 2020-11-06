using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterPanel : MonoBehaviour
{
    public Text level;
    public Text unitName;
    public Text health;
    public Text mana;
    public Text chargeTime;
    public Image portrait;
    [HideInInspector]
    public PanelPositioner panelPositioner;
    private void Awake()
    {
        panelPositioner = GetComponent<PanelPositioner>();
    }
    private void SetValues(Unit unit)
    {
        level.text = "Lv." + unit.level;
        unitName.text = unit.name;
        health.text = string.Format("Hp {0}/{1}", unit.stats[StatEnum.HP].currentValue, unit.stats[StatEnum.MaxHP].baseValue);
        mana.text = string.Format("MP {0}/{1}", unit.stats[StatEnum.MP].currentValue, unit.stats[StatEnum.MaxMP].baseValue);
        chargeTime.text = "Ct " + unit.chargeTime;
        portrait.sprite = unit.portrait;
    }
    public void Show(Unit unit, bool showController)
    {
        SetValues(unit);
        if(showController)
            panelPositioner.MoveTo("ShowWithController");
        else 
            panelPositioner.MoveTo("ShowDefault");
    }
    public void Hide()
    {
        panelPositioner.MoveTo("Hide");
    }
}
