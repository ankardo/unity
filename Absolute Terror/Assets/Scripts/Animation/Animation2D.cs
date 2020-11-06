using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Animation2D
{
    public string spriteAssetName;
    public string name;
    public float delay;
    public float frameRate;
    public List<Sprite> frames;
}
