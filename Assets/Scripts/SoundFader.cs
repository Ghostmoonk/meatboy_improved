using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class SoundFader : MonoBehaviour
{
    AudioSource source;
    [SerializeField] bool startFade = true;
    [SerializeField] float startFadeInDuration;
    [SerializeField] float startFadeInWaitDuration;

    [Range(0, 1f)]
    [SerializeField] float fadeInVolume;
    [Range(0, 1f)]
    [SerializeField] float fadeOutVolume;

    [SerializeField] Ease animationFadeInEase;
    [SerializeField] Ease animationFadeOutEase;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (startFade)
            StartCoroutine(WaitBeforeStartFade(startFadeInWaitDuration));
    }
    public void FadeIn(float duration)
    {
        source.DOFade(fadeInVolume, duration).SetEase(animationFadeInEase);
    }

    public void FadeOut(float duration)
    {
        source.DOFade(fadeOutVolume, duration).SetEase(animationFadeOutEase);
    }

    IEnumerator WaitBeforeStartFade(float durationWait)
    {
        yield return new WaitForSeconds(durationWait);
        FadeIn(startFadeInDuration);
    }
}
