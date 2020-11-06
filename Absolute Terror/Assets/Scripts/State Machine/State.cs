using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour
{
    protected int index;
    protected Image currentUISelector;
    protected InputController inputs { get { return InputController.instance; } }
    protected StateMachineController stMachine { get { return StateMachineController.instance; } }
    protected Board board { get { return Board.instance; } }
    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }

    protected void OnMoveTileSelector(object sender, object args)
    {
        Vector3Int input = (Vector3Int)args;
        LogicTile tile = Board.GetTile(Selector.instance.position + input);
        MoveSelector(tile);
    }

    protected void MoveSelector(Vector3Int pos)
    {
        MoveSelector(Board.GetTile(pos));
    }

    protected void MoveSelector(LogicTile tile)
    {
        if (tile != null)
        {
            Selector.instance.tile = tile;
            Selector.instance.spriteRenderer.sortingOrder = tile.contentOrder;
            Selector.instance.transform.position = tile.worldPos;
            stMachine.selectedTile = tile;
        }
    }

    protected void MoveUISelectorToCurrentIndex(List<Image> buttons)
    {
        if (index == -1)
            index = buttons.Count - 1;
        else if (index == buttons.Count)
            index = 0;

        currentUISelector.transform.localPosition =
            buttons[index].transform.localPosition;

    }
    protected void SetButtonStateColor(Image image, bool cantUse)
    {
        if (cantUse)
            image.color = Color.gray;
        else
            image.color = Color.white;
    }
    protected IEnumerator AIMoveSelector(Vector3Int destination)
    {
        while (Selector.instance.position != destination)
        {
            if (Selector.instance.position.x < destination.x)
                OnMoveTileSelector(null, Vector3Int.right);

            if (Selector.instance.position.x > destination.x)
                OnMoveTileSelector(null, Vector3Int.left);

            if (Selector.instance.position.y < destination.y)
                OnMoveTileSelector(null, Vector3Int.up);

            if (Selector.instance.position.y > destination.y)
                OnMoveTileSelector(null, Vector3Int.down);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
