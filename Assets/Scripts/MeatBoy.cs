using UnityEngine;

public class MeatBoy : Character
{
    #region Mouvements
    [SerializeField] float speed;

    #region Sauts
    [SerializeField] int jumpsMax = 2;
    int jumpWallCount = 0;
    int jumpsCount;
    #endregion

    #endregion

    #region Prefabs
    [SerializeField] GameObject gouttePrefab;
    [SerializeField] float delayGoutte;
    float cptGoutte;
    #endregion

    #region Respawn
    Vector3 defaultPosition;
    #endregion

    #region WallJump
    Wall lastWallJumped;
    #endregion

    private void Start()
    {
        defaultPosition = transform.position;
    }

    protected override void Update()
    {
        mouvement.x = Input.GetAxis("Horizontal") * speed;
        base.Update();

        if (controller.isGrounded)
        {
            if (jumpWallCount > 0)
                jumpWallCount = 0;

            mouvement.y = 0f;
            if (jumpsCount > 0)
                jumpsCount = 0;
        }

        #region Saut

        if (Input.GetButtonDown("Jump") && jumpsCount < jumpsMax + jumpWallCount)
        {
            mouvement.y = jumpSpeed;
            jumpsCount++;
        }

        #endregion

        controller.Move(mouvement * Time.deltaTime);

        if (mouvement.x != 0f)
        {
            cptGoutte -= Time.deltaTime;
        }

        if (cptGoutte < 0)
        {
            Goutte goutteInstance = Instantiate(gouttePrefab,
            new Vector3(transform.position.x,
            transform.position.y - GetComponent<Collider>().bounds.extents.y,
            transform.position.z),
            Quaternion.identity).GetComponent<Goutte>();
            goutteInstance.SetVelocity(new Vector3(-mouvement.x, -mouvement.y, goutteInstance.GetVelocity().z));
            cptGoutte = delayGoutte;
        }

        if (controller.isGrounded && lastWallJumped != null)
            lastWallJumped = null;
    }

    public void Die()
    {
        controller.enabled = false;
        transform.position = defaultPosition;
        controller.enabled = true;

        mouvement = Vector2.zero;
    }

    public Wall GetLastWallJumped() => lastWallJumped;

    public void SetLastWallJumped(Wall wall) => lastWallJumped = wall;

    public void AddJumpWallCount(int amount) => jumpWallCount += amount;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Wall>() && !controller.isGrounded)
        {
            if (lastWallJumped != hit.gameObject.GetComponent<Wall>())
            {
                AddJumpWallCount(1);
                SetLastWallJumped(hit.gameObject.GetComponent<Wall>());
            }
        }
    }
}