using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    public SpriteLoader unitSprites;
    SpriteRenderer spriteRenderer;
    public Animation2D currentAnimation;
    public Coroutine playing;
    public Queue<Animation2D> sequence;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sequence = new Queue<Animation2D>();
    }
    private IEnumerator Play()
    {
        while (true)
        {
            currentAnimation = sequence.Dequeue();
            if (sequence.Count == 0)
                sequence.Enqueue(currentAnimation);

            if (currentAnimation != null)
            {

                float timerPerFrame = 1 / currentAnimation.frameRate;

                for (int i = 0; i < currentAnimation.frames.Count; i++)
                {
                    spriteRenderer.sprite = currentAnimation.frames[i];
                    yield return new WaitForSeconds(timerPerFrame);
                }
            }
            yield return null;

        }
    }
    public void Stop()
    {
        if (playing != null)
            StopCoroutine(playing);
        sequence.Clear();
    }

    public void PlayAnimation(string name)
    {
        Stop();
        sequence.Enqueue(unitSprites.GetAnimation(name));
        playing = StartCoroutine(Play());
    }
    public void PlayAnimations(List<string> names)
    {
        Stop();
        foreach (string name in names)
        {
            sequence.Enqueue(unitSprites.GetAnimation(name));
        }
        playing = StartCoroutine(Play());
    }
    public void PlayThenReturn(string name) // used when it needs to stop the animation, like when the character gets hurt
    {
        Animation2D toPlay = unitSprites.GetAnimation(name);
        Stop();
        sequence.Enqueue(toPlay);
        sequence.Enqueue(currentAnimation);
        playing = StartCoroutine(Play());
    }
    public void PlayAtTheEnd(string name)
    {
        Animation2D toPlay = unitSprites.GetAnimation(name);
        sequence.Enqueue(toPlay);
    }
    public void PlayThenStop(string name)
    {
        Stop();
        sequence.Enqueue(unitSprites.GetAnimation(name));
        playing = StartCoroutine(PlayOnce());
    }
    public IEnumerator PlayOnce()
    {
        currentAnimation = sequence.Dequeue();

        float timerPerFrame = 1 / currentAnimation.frameRate;

        for (int i = 0; i < currentAnimation.frames.Count; i++)
        {
            spriteRenderer.sprite = currentAnimation.frames[i];
            yield return new WaitForSeconds(timerPerFrame);
        }
        yield return null;
    }
}
