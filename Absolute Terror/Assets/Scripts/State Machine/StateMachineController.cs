using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateMachineController : MonoBehaviour
{
    public static StateMachineController instance;
    private bool busy;
    public State current { get; private set; }
    public Transform selector;
    public LogicTile selectedTile;
    public List<Unit> units;

    [Header("ChooseActionState")]
    public List<Image> chooseActionButtons;
    public Image chooseActionSelector;
    public PanelPositioner chooseActionPanel;

    [Header("ChooseSkillState")]
    public List<Image> chooseSkillButtons;
    public Image chooseSkillSelector;
    public PanelPositioner chooseSkillPanel;
    public Sprite cantChooseSkill;

    [Header("Skill Prediction")]
    public SkillPredicitionPanel skillPredicitionPanel;

    [Header("Character")]
    public CharacterPanel leftCharacterPanel;
    public CharacterPanel rightCharacterPanel;

    [Header("Advance Job")]
    public AdvanceJobPanel advanceJobPanel;

    [Header("CombatEndState")]
    public PanelPositioner combatEndPanel;
    public TextMeshProUGUI combatEndText;

    [Header("Mobile Controllers")]
    public List<PanelPositioner> mobileControlPanels;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeTo<LoadState>();
    }
    public void ChangeTo<T>() where T : State
    {
        State state = GetState<T>();
        if (current != state)
            ChangeState(state);
    }
    private T GetState<T>() where T : State
    {
        T target = GetComponent<T>();
        if (target == null)
            target = gameObject.AddComponent<T>();

        return target;
    }
    protected void ChangeState(State value)
    {
        if (busy)
            return;

        busy = true;

        if (current != null)
            current.Exit();

        current = value;

        if (current != null)
            current.Enter();

        busy = false;
    }
}
