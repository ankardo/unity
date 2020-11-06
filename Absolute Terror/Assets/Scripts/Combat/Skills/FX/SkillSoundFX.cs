using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSoundFX : MonoBehaviour
{

    public AudioClip onlyOnHit;
    public AudioClip miss;
    public float delay;
    [HideInInspector]
    public bool didHit;
    public void Play()
    {
        if(didHit && onlyOnHit != null)
            Turn.unit.audioSource.clip = onlyOnHit;
        else if(!didHit && miss != null)
            Turn.unit.audioSource.clip = miss; 
        Turn.unit.audioSource.PlayDelayed(delay);
    }
}
