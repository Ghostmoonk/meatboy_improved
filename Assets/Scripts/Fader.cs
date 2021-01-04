using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class Fader : MonoBehaviour
{
    [SerializeField] UnityEvent OnFadeOutEnds;
    [SerializeField] UnityEvent OnFadeInEnds;
    [SerializeField] Ease animationFadeInEase;
    [SerializeField] Ease animationFadeOutEase;
    Graphic graphic;

    [SerializeField] bool startFade = true;
    [SerializeField] float startFadeOutDuration;
    [SerializeField] float startFadeOutWaitDuration;

    private void Start()
    {
        graphic = GetComponent<Graphic>();

        if (startFade)
            StartCoroutine(WaitBeforeStartFade(startFadeOutWaitDuration));

    }
    public void FadeIn(float duration)
    {
        graphic.DOColor(new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1), duration).SetEase(animationFadeInEase).OnComplete(() => { OnFadeInEnds?.Invoke(); });
    }

    public void FadeOut(float duration)
    {
        graphic.DOColor(new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0), duration).SetEase(animationFadeOutEase).OnComplete(() => { OnFadeOutEnds?.Invoke(); });
    }

    IEnumerator WaitBeforeStartFade(float durationWait)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1);
        yield return new WaitForSeconds(durationWait);
        FadeOut(startFadeOutDuration);
    }
}
