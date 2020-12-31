using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fin : MonoBehaviour
{
    [SerializeField] Chronometre chronoScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            chronoScript.End();
        }
    }
}
