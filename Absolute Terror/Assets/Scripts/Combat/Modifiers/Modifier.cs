using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : MonoBehaviour
{
    public float value;
    protected Unit host;
    public virtual void Activate(Unit unit)
    {
        host = unit;
    }
    protected abstract void Modify(object args);
    public abstract void Deactivate();
}
