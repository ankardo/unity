using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Unit unit;
    private SpriteSwapper spriteSwapper;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        spriteSwapper = transform.Find("Jumper/UnitSprite").GetComponent<SpriteSwapper>();

    }
    private void Play(string animationName)
    {
        animationName += unit.direction;
        if (spriteSwapper.currentAnimation.name != animationName)
            spriteSwapper.PlayAnimation(animationName);

    }
    public void Idle()
    {
        Play("Idle");
    }
    public void Walk()
    {
        Play("Walk");
    }
    public void Attack(string attackAnimationName)
    {
        //Animation2D characterUniqueAnimation = spriteSwapper.unitSprites.GetAnimation(attackAnimationName + unit.direction);
        if (string.IsNullOrEmpty(attackAnimationName))
            spriteSwapper.PlayThenReturn("Attack" + unit.direction);
        else
            spriteSwapper.PlayThenReturn(attackAnimationName + unit.direction);

    }
    public void GotHit()
    {
        spriteSwapper.PlayThenReturn("GotHit" + unit.direction);
    }
    public void GotHit(float hitDelay)
    {
        Invoke("GotHit", hitDelay);
    }
    public void Death()
    {
        spriteSwapper.PlayThenStop("Death" + unit.direction);
    }
    public void Death(float hitDelay)
    {
        Invoke("Death", hitDelay);
    }
    public float GetAnimationTimer(string animationName)
    {
        Animation2D animation = spriteSwapper.unitSprites.GetAnimation(animationName);
        float timePerFrame = 1 / animation.frameRate;

        return animation.frames.Count * timePerFrame;
    }
    public float Jump()
    {
        Play("Jump");
        return GetAnimationTimer("Jump" + unit.direction);
    }

}
