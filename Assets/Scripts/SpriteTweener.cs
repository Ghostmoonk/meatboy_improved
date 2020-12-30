using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTweener : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Color endColor;
    [SerializeField] float duration;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    public void TweenColor()
    {
        spriteRenderer.DOColor(endColor, duration);
    }
}
