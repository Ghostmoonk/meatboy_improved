using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{
    #region Components
    protected Rigidbody2D rb2D { get; set; }
    protected Animator animator { get; set; }
    protected Collider2D col2D { get; set; }
    #endregion

    #region Mouvements
    [Header("Character movements")]
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpSpeed;

    protected int lookingDirection = 1;
    protected Vector2 mouvement;
    #endregion

    [SerializeField] protected GroundChecker groundChecker;

    protected bool isDead { get; set; }
    protected bool isGrounded { get; set; }

    public abstract void Jump();

    public abstract void Die();

    public abstract void Move(float horizontalAxis);

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();

        isDead = false;
        isGrounded = false;
    }
}
