using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    [SerializeField] Tilemap tilemap;
    [SerializeField] TilemapCollider2D tilemapCol;

    [SerializeField] private Collider2D col2D;
    [SerializeField] private float rayDist = 0.1f;

    public bool IsWalled { get { return m_IsWalled; } }

    bool m_IsWalled = false;

    Side sideToCheck = Side.Right;
    Side sideWalled;

    List<Vector3Int> lastTilesPosCol = new List<Vector3Int>();

    private void Update()
    {
        Vector2 top = Vector2.zero;
        Vector2 center = Vector2.zero;
        Vector2 bottom = Vector2.zero;
        bool leftWalled = false;
        bool rightWalled = false;

        top = new Vector2(col2D.bounds.center.x, col2D.bounds.max.y);
        center = new Vector2(col2D.bounds.center.x, col2D.bounds.center.y);
        bottom = new Vector2(col2D.bounds.center.x, col2D.bounds.min.y + 0.01f);


        RaycastHit2D rightHitTop = Physics2D.Raycast(top + new Vector2(col2D.bounds.extents.x, 0f), new Vector2(1f, 0f), rayDist, wallMask);
        RaycastHit2D rightHitCenter = Physics2D.Raycast(center + new Vector2(col2D.bounds.extents.x, 0f), new Vector2(1f, 0f), rayDist, wallMask);
        RaycastHit2D rightHitBottom = Physics2D.Raycast(bottom + new Vector2(col2D.bounds.extents.x, 0f), new Vector2(1f, 0f), rayDist, wallMask);

        Debug.DrawRay(top + new Vector2(col2D.bounds.extents.x, 0f), new Vector2(rayDist, 0f), Color.blue, Time.deltaTime);
        Debug.DrawRay(center + new Vector2(col2D.bounds.extents.x, 0f), new Vector2(rayDist, 0f), Color.blue, Time.deltaTime);
        Debug.DrawRay(bottom + new Vector2(col2D.bounds.extents.x, 0f), new Vector2(rayDist, 0f), Color.blue, Time.deltaTime);

        RaycastHit2D leftHitTop = Physics2D.Raycast(top - new Vector2(col2D.bounds.extents.x, 0f), new Vector2(-1f, 0f), rayDist, wallMask);
        RaycastHit2D leftHitCenter = Physics2D.Raycast(center - new Vector2(col2D.bounds.extents.x, 0f), new Vector2(-1f, 0f), rayDist, wallMask);
        RaycastHit2D lefttHitBottom = Physics2D.Raycast(bottom - new Vector2(col2D.bounds.extents.x, 0f), new Vector2(-1f, 0f), rayDist, wallMask);

        Debug.DrawRay(top - new Vector2(col2D.bounds.extents.x, 0f), new Vector2(-rayDist, 0f), Color.blue, Time.deltaTime);
        Debug.DrawRay(center - new Vector2(col2D.bounds.extents.x, 0f), new Vector2(-rayDist, 0f), Color.blue, Time.deltaTime);
        Debug.DrawRay(bottom - new Vector2(col2D.bounds.extents.x, 0f), new Vector2(-rayDist, 0f), Color.blue, Time.deltaTime);


        leftWalled = leftHitTop.collider != null || leftHitCenter.collider != null || lefttHitBottom.collider != null;
        rightWalled = rightHitTop.collider != null || rightHitCenter.collider != null || rightHitBottom.collider != null;

        if (rightWalled)
        {
            sideWalled = Side.Right;

            List<Vector3Int> contactTilesPos = GetYSurroundedTiles(new Vector3Int((int)rightHitCenter.point.x, (int)rightHitCenter.point.y, 0));
            if (!lastTilesPosCol.Contains(contactTilesPos[0]))
            {
                lastTilesPosCol = contactTilesPos;
                onNewWalled();
            }
        }
        else if (leftWalled)
        {
            sideWalled = Side.Left;
            List<Vector3Int> contactTilesPos = GetYSurroundedTiles(new Vector3Int((int)leftHitCenter.point.x - 1, (int)leftHitCenter.point.y, 0));

            if (!lastTilesPosCol.Contains(contactTilesPos[0]))
            {
                lastTilesPosCol = contactTilesPos;
                onNewWalled();
            }
        }
        bool walled = leftWalled || rightWalled;

        if (walled && !m_IsWalled)
        {
            if (OnWalled != null)
            {
                OnWalled();
            }
        }

        m_IsWalled = leftWalled || rightWalled;
    }

    List<Vector3Int> GetYSurroundedTiles(Vector3Int initialTilePos)
    {
        List<Vector3Int> tilesPos = new List<Vector3Int>();
        int increment = 1;
        bool checkTop = true;
        bool checkBottom = true;
        tilesPos.Add(initialTilePos);
        do
        {
            if (checkBottom)
            {
                TileBase newTile = tilemap.GetTile(new Vector3Int(initialTilePos.x, initialTilePos.y - increment, initialTilePos.z));
                if (newTile != null)
                {
                    tilesPos.Add(new Vector3Int(initialTilePos.x, initialTilePos.y - increment, initialTilePos.z));
                }
                else
                {
                    checkBottom = false;
                }
            }

            if (checkTop)
            {
                TileBase newTile = tilemap.GetTile(new Vector3Int(initialTilePos.x, initialTilePos.y + increment, initialTilePos.z));
                if (newTile != null)
                {
                    tilesPos.Add(new Vector3Int(initialTilePos.x, initialTilePos.y + increment, initialTilePos.z));
                }
                else
                {
                    checkTop = false;
                }
            }
            increment += 1;
        } while (checkTop || checkBottom);

        return tilesPos;
    }

    public void ResetLastWall() => lastTilesPosCol.Clear();

    public void SetSideToCheck(Side newSideToCheck) => sideToCheck = newSideToCheck;

    public Side GetSideWalled() => sideWalled;

}
