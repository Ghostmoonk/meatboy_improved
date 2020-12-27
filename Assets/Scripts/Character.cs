using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Character : MonoBehaviour
{
    #region Components
    protected CharacterController controller;
    #endregion

    #region Mouvements
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected float gravity;
    protected Vector3 mouvement;
    #endregion

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        mouvement.y -= gravity * Time.deltaTime;
    }
}
