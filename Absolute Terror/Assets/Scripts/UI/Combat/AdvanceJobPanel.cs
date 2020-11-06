using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceJobPanel : MonoBehaviour
{
    public List<JobPanelInfo> jobPanelInfos;
    public RectTransform selector;
    private int index;
    private PanelPositioner panelPositioner;
    private void Awake()
    {
        panelPositioner = GetComponent<PanelPositioner>();
    }
    public void Show()
    {
        index = 0;
        ChangeSelected();
        UpdatePanelInfo();
        panelPositioner.MoveTo("Show");
    }
    public void Hide()
    {
        panelPositioner.MoveTo("Hide");
    }
    public void SelectNext()
    {
        index++;
        if (index >= jobPanelInfos.Count)
            index = 0;
        ChangeSelected();
    }
    public void SelectPrevious()
    {
        index--;
        if (index < 0)
            index = jobPanelInfos.Count - 1;
        ChangeSelected();
    }
    public void DuplicatePanel()
    {
        Vector3 newPos = jobPanelInfos[0].panel.transform.position;
        newPos.x += 120 * jobPanelInfos.Count;
        GameObject instance = Instantiate(jobPanelInfos[0].panel, newPos, Quaternion.identity, transform);
        JobPanelInfo newPanelInfo = instance.GetComponent<JobPanelInfo>();
        jobPanelInfos.Add(newPanelInfo);
        selector.gameObject.transform.SetAsLastSibling();
    }

    public void JobChange()
    {
        Job.Employ(Turn.unit, Turn.unit.job.advancesTo[index], 5);

    }
    private void ChangeSelected()
    {
        selector.position = jobPanelInfos[index].panel.transform.position;
    }
    private void UpdatePanelInfo()
    {
        Clean();
        for (int i = 0; i < Turn.unit.job.advancesTo.Count; i++)
        {
            Job job = Turn.unit.job.advancesTo[i];
            jobPanelInfos[i].jobName.text = job.name;
            jobPanelInfos[i].description.text = job.description;
            jobPanelInfos[i].portrait.sprite = job.portrait;
            if (Turn.unit.job.advancesTo.Count > jobPanelInfos.Count)
                DuplicatePanel();
        }
    }
    private void Clean()
    {
        for (int i = jobPanelInfos.Count - 1; i > 0; i--)
        {
            JobPanelInfo jobPanelInfo = jobPanelInfos[i];
            jobPanelInfos.Remove(jobPanelInfo);
            Destroy(jobPanelInfo.panel);
        }

    }
}