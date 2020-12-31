using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DieScreen : MonoBehaviour
{
    [SerializeField] UnityEvent OnDieScreenFull;

    public void FireDieScreenFullEvents() => OnDieScreenFull?.Invoke();

}
