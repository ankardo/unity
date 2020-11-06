using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void InputDelegateModel(object sender, object args);

public class InputController : MonoBehaviour
{
    float hCooldown = 0;
    float vCooldown = 0;

    float cooldownTimer = 0.5f;

    public static InputController instance;

    public InputDelegateModel OnMove;
    public InputDelegateModel OnFire;

    public bool boardAxes;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        int h = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        int v = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        Vector3Int moved = new Vector3Int(0, 0, 0);

        if (h != 0 && !boardAxes)
            moved.x = GetMoved(ref hCooldown, h);
        else
        {
            if (h != 0 && boardAxes)
                moved.y = GetMoved(ref hCooldown, h * -1);
            else
                hCooldown = 0;
        }

        if (v != 0 && !boardAxes)
            moved.y = GetMoved(ref vCooldown, v);
        else
        {
            if (v != 0 && boardAxes)
                moved.x = GetMoved(ref vCooldown, v);
            else
                vCooldown = 0;
        }

        if (moved != Vector3.zero && OnMove != null)
            OnMove(null, moved);

        if (Input.GetButtonDown("Submit") && OnFire != null)
            OnFire(null, 1);

        if ((Input.GetButtonDown("Fire2") || Input.GetButtonDown("Cancel")) && OnFire != null)
            OnFire(null, 2);

        if (Input.GetButtonDown("Print Combat Log"))
            CombatLog.PrintCombatLog();
    }
    public List<Button> movementButtons;
    public delegate void ButtonClickModel(int args);
    public ButtonClickModel OnButtonClicked;
    private void Start()
    {
        if (movementButtons.Count > 0)
        {
            movementButtons[0].onClick.AddListener(() => handleMovementButton(Vector3Int.up));
            movementButtons[1].onClick.AddListener(() => handleMovementButton(Vector3Int.down));
            movementButtons[2].onClick.AddListener(() => handleMovementButton(Vector3Int.left));
            movementButtons[3].onClick.AddListener(() => handleMovementButton(Vector3Int.right));
        }
    }
    public void HandleButton(int button)
    {
        OnFire(null, button);
    }
    public void handleMovementButton(Vector3Int direction)
    {
        if (boardAxes)
        {
            if (direction == Vector3Int.up)
                OnMove(null, Vector3Int.right);
            else
            {
                if (direction == Vector3Int.down)
                    OnMove(null, Vector3Int.left);
                else
                {
                    if (direction == Vector3Int.left)
                        OnMove(null, Vector3Int.up);
                    else
                    {
                        if (direction == Vector3Int.right)
                            OnMove(null, Vector3Int.down);
                    }
                }
            }
        }
        else
            OnMove(null, direction);

    }
    public void ButtonClicked(int args)
    {
        if (OnButtonClicked != null)
        {
            OnButtonClicked(args);
        }
    }
    private int GetMoved(ref float cooldownSum, int value)
    {
        if (Time.time > cooldownSum)
        {
            cooldownSum += Time.time + cooldownTimer;
            return value;
        }
        return 0;
    }
}
