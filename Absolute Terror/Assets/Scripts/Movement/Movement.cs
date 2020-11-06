using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    const float MoveSpeed = 0.5f;
    const float JumpHeight = 0.5f;
    readonly Vector3 SmallShadow = new Vector3(0.1f, 0.1f, 0.5f);

    public List<Vector3Int> path;
    SpriteRenderer characterSpriteRenderer;
    SpriteRenderer shadowSpriteRenderer;
    Transform jumper;
    Transform shadow;
    LogicTile currentTile;

    private void Awake()
    {
        jumper = transform.Find("Jumper");
        shadow = transform.Find("Shadow");
        characterSpriteRenderer = transform.Find("Jumper/UnitSprite").GetComponent<SpriteRenderer>(); //GetComponentInChildren<SpriteRenderer>();
        shadowSpriteRenderer = shadow.GetComponent<SpriteRenderer>();
    }

    private IEnumerator Walk(LogicTile to)
    {
        int id = LeanTween.move(transform.gameObject, to.worldPos, MoveSpeed).id;
        yield return new WaitForSeconds(MoveSpeed * 0.5f);
        currentTile = to;
        characterSpriteRenderer.sortingOrder = to.contentOrder;
        shadowSpriteRenderer.sortingOrder = to.contentOrder;

        while (LeanTween.descr(id) != null)
        {
            yield return null;
        }
    }

    private IEnumerator Jump(LogicTile to, float duration)
    {
        yield return new WaitForSeconds(0.15f);
        float timerOrderUpdate = duration;
        if (currentTile.floor.tilemap.tileAnchor.y > to.floor.tilemap.tileAnchor.y)
            timerOrderUpdate *= 0.85f;
        else
            timerOrderUpdate *= 0.2f;

        int id = LeanTween.move(transform.gameObject, to.worldPos, duration).id;

        LeanTween.moveLocalY(jumper.gameObject, JumpHeight, duration * 0.5f).setLoopPingPong(1).setEase(LeanTweenType.easeInOutQuad);

        LeanTween.scale(shadow.gameObject, SmallShadow, duration * 0.5f).setLoopPingPong(1).setEase(LeanTweenType.easeInOutQuad);

        yield return new WaitForSeconds(timerOrderUpdate);

        currentTile = to;
        characterSpriteRenderer.sortingOrder = to.contentOrder;
        shadowSpriteRenderer.sortingOrder = to.contentOrder;

        while (LeanTween.descr(id) != null)
        {
            yield return null;
        }

    }

    public IEnumerator Move(List<LogicTile> path)
    {
        currentTile = Turn.unit.tile;
        currentTile.content = null;

        for (int i = 0; i < path.Count; i++)
        {
            LogicTile to = path[i];

            Turn.unit.direction = currentTile.GetDirection(to);

            if (currentTile.floor != to.floor)
            {
                float duration = Turn.unit.animationController.Jump();
                yield return StartCoroutine(Jump(to, duration));
            }
            else
            {
                Turn.unit.animationController.Walk();
                yield return StartCoroutine(Walk(to));
            }
        }
        Turn.unit.animationController.Idle();
    }
    public virtual bool CanMoveFromTo(LogicTile from, LogicTile to)
    {
        to.distance = from.distance + 1;
        return !(to.content != null || to.distance > Turn.unit.GetStat(StatEnum.MOV) || Mathf.Abs(from.floor.height - to.floor.height) > 1);
    }
}
