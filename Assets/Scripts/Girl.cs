using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Girl : Character
{
    [SerializeField] float rotateSpeed = 5f;
    protected override void Update()
    {
        base.Update();

        if (controller.isGrounded)
        {
            mouvement.y = jumpSpeed;
        }
        controller.Move(mouvement * Time.deltaTime);
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);
    }
}
