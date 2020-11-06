using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkillBehaviour : MonoBehaviour
{
    private int index;
    public List<SkillPicker> pickers;
    public void Pick(AIPlan plan)
    {
        pickers[index].Pick(plan);
        index++;
        if (index >= pickers.Count)
            index = 0;
    }
    private void Awake()
    {
        pickers = new List<SkillPicker>();
        pickers.AddRange(GetComponent<Unit>().job.AISkillPicks.GetComponents<SkillPicker>());
    }
}
