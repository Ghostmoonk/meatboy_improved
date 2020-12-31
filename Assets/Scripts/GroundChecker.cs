using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public delegate void GroundedHandler();

    public event GroundedHandler OnGrounded;

    [SerializeField] LayerMask groundMask;

    [SerializeField]
    private Collider2D col2D;

    [SerializeField]
    private float rayDistance = 0.5f;
    const string GROUND_LAYER = "Ground";

    public bool IsGrounded { get { return m_IsGrounded; } }

    private bool m_IsGrounded = false;

    void Awake()
    {
        m_IsGrounded = false;
    }

    void Update()
    {
        Vector2 left = new Vector2(col2D.bounds.max.x - 0.01f, col2D.bounds.center.y);
        Vector2 center = new Vector2(col2D.bounds.center.x, col2D.bounds.center.y);
        Vector2 right = new Vector2(col2D.bounds.min.x + 0.01f, col2D.bounds.center.y);

        RaycastHit2D hit1 = Physics2D.Raycast(left, new Vector2(0f, -1f), rayDistance, groundMask);
        Debug.DrawRay(left, new Vector2(0f, -rayDistance), Color.blue, Time.deltaTime);
        bool grounded1 = hit1.collider != null;

        RaycastHit2D hit2 = Physics2D.Raycast(center, new Vector2(0f, -1f), rayDistance, groundMask);
        Debug.DrawRay(center, new Vector2(0f, -rayDistance), Color.red, Time.deltaTime);
        bool grounded2 = hit2.collider != null;

        RaycastHit2D hit3 = Physics2D.Raycast(right, new Vector2(0f, -1f), rayDistance, groundMask);
        Debug.DrawRay(right, new Vector2(0f, -rayDistance), Color.black, Time.deltaTime);
        bool grounded3 = hit3.collider != null;

        bool grounded = grounded1 || grounded2 || grounded3;

        if (grounded && !m_IsGrounded)
        {
            if (OnGrounded != null)
            {
                OnGrounded();
            }
        }

        m_IsGrounded = grounded;
    }

}
