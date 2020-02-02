using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fadeInText : MonoBehaviour
{
    public float time;
    public TextMeshProUGUI text;

    private void Start()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    public void FadeInFadeOut()
    {
        StartCoroutine(fadeInFadeOut());
    }

    private IEnumerator fadeInFadeOut ()
    {
        float timer = 0;

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (timer * 2 < time)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, timer / time * 2);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timer = 0;
        while (timer * 2 < time)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - timer / time * 2);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }
}
