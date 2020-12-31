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
        if (collision.GetComponent<MeatBoy>() && !triggered)
        {
            OnTrigger?.Invoke();
            triggered = true;

            collision.GetComponent<MeatBoy>().SetCheckpointPosition(transform.position);
            collision.GetComponent<MeatBoy>().ResetVelocity();
        }
    }
}
