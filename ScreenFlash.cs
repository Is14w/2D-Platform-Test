using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    [SerializeField] public Image redFilterImg;
    [SerializeField] private float flashTime;
    [SerializeField] public Color flashColor;
    private Color defaultColor;

    private void Start()
    {
        defaultColor = redFilterImg.color;
    }

    public void FlashScreen()
    {
        StartCoroutine(StartFlash()); 
    }

    IEnumerator StartFlash()
    {
        for (float i = flashTime; i < flashTime * 30; i += flashTime)
        {
            redFilterImg.color = Color.Lerp(redFilterImg.color, flashColor, 0.05f);
            yield return new WaitForSeconds(flashTime);
        }
        for (float i = flashTime; i < flashTime * 100; i += flashTime)
        {
            redFilterImg.color = Color.Lerp(redFilterImg.color, defaultColor, 0.05f);
            yield return new WaitForSeconds(flashTime);
        }
    }
}
