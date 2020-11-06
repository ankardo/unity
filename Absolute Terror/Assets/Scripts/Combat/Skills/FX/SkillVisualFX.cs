using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillVisualFX : MonoBehaviour
{

    public Vector3 offset;
    public ParticleSystem always;
    public ParticleSystem onlyOnHit;
    public ParticleSystem miss;
    [HideInInspector]
    public Unit target;
    public float delay;
    [HideInInspector]
    public bool didHit;
    public void VFX()
    {
        Invoke("VFXDelay", delay);
    }
    private void VFXDelay()
    {
        if (always != null)
            SpawnEffect(always);
        if (didHit && onlyOnHit != null)
            SpawnEffect(onlyOnHit);
        if (!didHit && miss != null)
            SpawnEffect(miss);
            
    }
    private void SpawnEffect(ParticleSystem toSpawn)
    {
        Instantiate(toSpawn, target.spriteSwapper.transform.position + offset,
                Quaternion.identity, target.spriteSwapper.transform);
    }
}
