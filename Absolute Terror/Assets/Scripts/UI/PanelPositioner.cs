using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPositioner : MonoBehaviour
{
    public List<PanelPosition> positions;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void MoveTo(string positionName)
    {
        StopAllCoroutines();
        LeanTween.cancel(this.gameObject);
        PanelPosition panelPos = positions.Find(pos => pos.name == positionName);
        StartCoroutine(Move(panelPos));
    }
    private IEnumerator Move(PanelPosition panelPos)
    {
        rect.anchorMax = panelPos.anchorMax;
        rect.anchorMin = panelPos.anchorMin;

        int id = LeanTween.move(rect, panelPos.position, 0.5f).id;

        while(LeanTween.descr(id) != null)
        {
            yield return null;
        }
    }
}
