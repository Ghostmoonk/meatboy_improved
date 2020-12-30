using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] SpriteTweener spriteTweener;
    bool triggered = false;
    [SerializeField] UnityEvent OnTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Character>() && !triggered)
        {
            OnTrigger?.Invoke();
            triggered = true;
        }
    }
}
