using System.Collections;
using UnityEngine;
using TMPro;
public class CombatText : MonoBehaviour
{
    public static CombatText instance;
    public TextMeshProUGUI prefab;
    public Vector3 offset;
    public Vector3 random;
    public float timeToLive;
    private void Awake()
    {
        instance = this;
    }
    [ContextMenu("Pop")]
    public void PopText(Unit unit, int value, StatEnum statName)
    {
        StartCoroutine(PopControl(unit, value, statName, null, Color.white));
    }
    public void PopText(Unit unit, string text, Color color)
    {
        StartCoroutine(PopControl(unit, 0, null, text, color));
    }
    private IEnumerator PopControl(Unit unit, int value, StatEnum? statName, string text, Color color)
    {
        yield return null;
        Vector3 randomPos = new Vector3(Random.Range(-random.x, random.x), Random.Range(-random.y, random.y), 0);
        TextMeshProUGUI instance =
            Instantiate(prefab, unit.transform.position + offset + randomPos, Quaternion.identity, unit.transform.Find("UI"));
        instance.transform.SetAsLastSibling();
        if (text != null)
        {
            instance.color = color;
            instance.text = text;
        }
        else
        {

            if (value <= 0)
            {
                if (statName == StatEnum.HP)
                    instance.color = Color.red;
                else
                    instance.color = Color.gray;
            }
            else
            {
                if (statName == StatEnum.HP)
                    instance.color = Color.green;
                else
                    instance.color = Color.blue;
            }
            if (value == 0)
            {
                instance.text = "";
            }
            else
                instance.text = "" + Mathf.Abs(value);
        }
        LeanTween.alphaCanvas(instance.GetComponent<CanvasGroup>(), 1, 0.5f);

        int id = LeanTween.moveY(instance.gameObject, instance.transform.localPosition.y + 3, timeToLive).id;
        yield return new WaitForSeconds(timeToLive * 0.5f);

        while (LeanTween.descr(id) != null)
        {
            yield return null;
        }
        LeanTween.alphaCanvas(instance.GetComponent<CanvasGroup>(), 0, 1).setOnComplete(
            () => Destroy(instance.gameObject)
        );
    }
}
