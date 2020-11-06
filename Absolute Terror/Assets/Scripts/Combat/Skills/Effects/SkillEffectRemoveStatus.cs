using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectRemoveStatus : SkillEffect
{

    public List<string> combatStatusNames;

    public override int Predict(Unit target)
    {
        return 0;
    }
    public override void Apply(Unit target)
    {
        Transform holder = target.transform.Find("Status");
        foreach (string combatStatusName in combatStatusNames)
        {
            SeekAndDestroy(combatStatusName, holder);
        }
    }
    private void SeekAndDestroy(string combatStatusName, Transform holder)
    {
        Transform combatStatus = holder.transform.Find(combatStatusName);
        if(combatStatus != null)
            Destroy(combatStatus.gameObject);
    }

}
