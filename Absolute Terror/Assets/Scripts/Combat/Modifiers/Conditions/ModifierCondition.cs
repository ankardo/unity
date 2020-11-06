using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifierCondition : ScriptableObject
{
    public virtual bool Validate(object arg1)
    {
        return false;
    }
    public virtual bool Validate(object arg1, object arg2)
    {
        return false;
    }
}
