
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class SpriteLoader : MonoBehaviour
{
    public static readonly List<string> animationNames =
        new List<string>() { "Idle", "Walk", "Attack", "GotHit", "Death", "Jump" };
    public static readonly List<char> directions
        = new List<char>() { 'N', 'S', 'E', 'W' };
    public static Transform holder;

    public List<Animation2D> animations;
    public Dictionary<string, Animation2D> animationsFinder;
    public float defaultFrameRate;
    public float defaultDelay;
    
    public AssetReference animationReference;

    private void Awake()
    {
        if (holder == null)
            holder = transform.parent;
    }


    public IEnumerator Load()
    {
        
        animations = new List<Animation2D>();
        animationsFinder = new Dictionary<string, Animation2D>();
        foreach (string animationName in animationNames)
        {
            for (int i = 0; i < directions.Count; i++)
            {

                Animation2D tempAnimation2D = new Animation2D();
                tempAnimation2D.frameRate = defaultFrameRate;
                tempAnimation2D.delay = defaultDelay;
                tempAnimation2D.spriteAssetName =  transform.name + '/' + animationName + directions[i];
                tempAnimation2D.name = animationName + directions[i];
                
                tempAnimation2D.frames = new List<Sprite>();

                yield return StartCoroutine(LoadFrames(tempAnimation2D));

                animations.Add(tempAnimation2D);
                animationsFinder.Add(tempAnimation2D.name, tempAnimation2D);
            }
        }
    }
    private IEnumerator LoadFrames(Animation2D animation)
    {
        Addressables.LoadAssetsAsync<Sprite>(animation.spriteAssetName, null).Completed += response =>
        {
            if(response.Result.Count > 0)
                animation.frames.AddRange(response.Result);
        };
        yield return null;
    }
    public Animation2D GetAnimation(string name)
    {
        animationsFinder.TryGetValue(name, out Animation2D retrieve);
        return retrieve;
    }

}

