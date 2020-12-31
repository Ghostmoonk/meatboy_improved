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

    List<Vector3Int> lastTilesPosCol = new List<Vector3Int>();

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

            if (walled2)
            {
                List<Vector3Int> contactTilesPos = GetYSurroundedTiles(new Vector3Int((int)hit2.point.x, (int)hit2.point.y, 0));
                if (!lastTilesPosCol.Contains(contactTilesPos[0]))
                {
                    lastTilesPosCol = contactTilesPos;
                    Debug.Log("New walled");
                    onNewWalled();
                }
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

            if (walled2)
            {
                List<Vector3Int> contactTilesPos = GetYSurroundedTiles(new Vector3Int((int)hit2.point.x - 1, (int)hit2.point.y, 0));

                if (!lastTilesPosCol.Contains(contactTilesPos[0]))
                {
                    lastTilesPosCol = contactTilesPos;
                    onNewWalled();
                }
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

}
