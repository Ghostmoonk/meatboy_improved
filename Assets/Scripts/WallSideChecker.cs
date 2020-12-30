using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Side
{
    Right, Left, Top, Bottom
}

public class WallSideChecker : MonoBehaviour
{
    public delegate void WalledHandler();
    public delegate void NewWallTouched();

    public event WalledHandler OnWalled;
    public event NewWallTouched onNewWalled;

    [SerializeField] LayerMask wallMask;

    [SerializeField] private Collider2D col2D;
    [SerializeField] private float rayDist = 0.1f;

    public bool IsWalled { get { return m_IsWalled; } }

    bool m_IsWalled = false;

    Side sideToCheck = Side.Right;

    Collider2D lastWallCol = null;
    Collider2D currentWallCol = null;

    private void Update()
    {
        Vector2 top = Vector2.zero;
        Vector2 center = Vector2.zero;
        Vector2 bottom = Vector2.zero;
        bool walled1 = false;
        bool walled2 = false;
        bool walled3 = false;
        if (sideToCheck == Side.Right)
        {
            top = new Vector2(col2D.bounds.max.x, col2D.bounds.max.y);
            center = new Vector2(col2D.bounds.max.x, col2D.bounds.center.y);
            bottom = new Vector2(col2D.bounds.max.x, col2D.bounds.min.y + 0.01f);
            RaycastHit2D hit1 = Physics2D.Raycast(top, new Vector2(1f, 0f), rayDist, wallMask);
            Debug.DrawRay(top, new Vector2(rayDist, 0f), Color.blue, Time.deltaTime);

            walled1 = hit1.collider != null;

            RaycastHit2D hit2 = Physics2D.Raycast(center, new Vector2(1f, 0f), rayDist, wallMask);
            Debug.DrawRay(center, new Vector2(rayDist, 0f), Color.red, Time.deltaTime);
            walled2 = hit2.collider != null;

            currentWallCol = walled2 ? hit2.collider : null;

            if (currentWallCol != lastWallCol && currentWallCol != null)
            {
                lastWallCol = currentWallCol;
                onNewWalled();
            }

            RaycastHit2D hit3 = Physics2D.Raycast(bottom, new Vector2(1f, 0f), rayDist, wallMask);
            Debug.DrawRay(bottom, new Vector2(rayDist, 0f), Color.black, Time.deltaTime);
            walled3 = hit3.collider != null;
        }
        else
        {
            top = new Vector2(col2D.bounds.min.x, col2D.bounds.max.y);
            center = new Vector2(col2D.bounds.min.x, col2D.bounds.center.y);
            bottom = new Vector2(col2D.bounds.min.x, col2D.bounds.min.y + 0.01f);

            RaycastHit2D hit1 = Physics2D.Raycast(top, new Vector2(-1f, 0f), rayDist, wallMask);
            Debug.DrawRay(top, new Vector2(-rayDist, 0f), Color.blue, Time.deltaTime);

            walled1 = hit1.collider != null;

            RaycastHit2D hit2 = Physics2D.Raycast(center, new Vector2(-1f, 0f), rayDist, wallMask);
            Debug.DrawRay(center, new Vector2(-rayDist, 0f), Color.red, Time.deltaTime);
            walled2 = hit2.collider != null;

            currentWallCol = walled2 ? hit2.collider : null;

            if (currentWallCol != lastWallCol && currentWallCol != null)
            {
                lastWallCol = currentWallCol;
                onNewWalled();
            }

            RaycastHit2D hit3 = Physics2D.Raycast(bottom, new Vector2(-1f, 01f), rayDist, wallMask);
            Debug.DrawRay(bottom, new Vector2(-rayDist, 0f), Color.black, Time.deltaTime);
            walled3 = hit3.collider != null;
        }

        bool walled = walled1 || walled2 || walled3;

        if (walled && !m_IsWalled)
        {
            if (OnWalled != null)
            {
                OnWalled();
            }
        }

        m_IsWalled = walled;
    }

    public void SetSideToCheck(Side newSideToCheck) => sideToCheck = newSideToCheck;

    public void ResetLastWall() => lastWallCol = null;
}
