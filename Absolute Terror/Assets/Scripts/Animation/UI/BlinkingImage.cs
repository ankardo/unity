using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour
{
    private Image flashingImage;
    private Color imageColor;
    private string currentAction;
    private void Start()
    {
        flashingImage = GetComponent<Image>();
        if (flashingImage != null)
        {
            imageColor = flashingImage.color;
        }
        StartCoroutine(BlinkImage());
    }

    private IEnumerator BlinkImage()
    {

        imageColor.a = 0;
        currentAction = "add";
        flashingImage.color = imageColor;
        while (true)
        {
            ChangeColorAlpha(.08f);
            yield return new WaitForSeconds(.25f);
        }
    }

    private void ChangeColorAlpha(float value)
    {
        if (imageColor != null)
        {
            if (imageColor.a >= .4f)
                currentAction = "remove";
            if (imageColor.a <= .2f)
                currentAction = "add";
            if (currentAction == "add")
                imageColor.a += value;
            else
                imageColor.a -= value;

            flashingImage.color = imageColor;
        }
    }
}
