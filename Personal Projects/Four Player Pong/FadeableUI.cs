using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
public class FadeableUI : MonoBehaviour
{
    // Canvas group and graphic raycast variables
    private CanvasGroup canvasGroup;
    private GraphicRaycaster raycast;

    // Time to fade
    [SerializeField] private float fadeTime = .5f;

    // Coroutine for fading
    private Coroutine fadeCoroutine;

    // Sets canvas groups and graphic raycast
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        raycast = GetComponent<GraphicRaycaster>();
    }

    // Determines if fading in or out
    public void FadeIn(bool instant)
    {
        raycast.enabled = true;
        Fade(1f, instant);
    }

    public void FadeOut(bool instant)
    {
        raycast.enabled = false;
        Fade(0f, instant);
    }

    // Activates fading
    private void Fade(float targetAlpha, bool instant)
    {
        if(instant)
        {
            canvasGroup.alpha = targetAlpha;
        }
        else
        {
            if(fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
        }
    }

    // Routine for fading
    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;

        for(float timer = 0; timer < fadeTime; timer += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
        fadeCoroutine = null;
    }
}
