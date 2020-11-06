using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AnimationBuilderLoader : MonoBehaviour
{
    public List<string> pathToLoad;

    [System.Obsolete]
    private void Start()
    {
        Addressables.LoadAssetsAsync<GameObject>(pathToLoad.ToArray(),
           obj =>
           {
               var newObject = Instantiate(obj, transform.position, transform.rotation);
               newObject.transform.parent = transform;
           },
           Addressables.MergeMode.Union);

    }
}
