using UnityEngine;

public class MeatBoy : Character
{
    #region Jumps
    [Header("Jump")]
    [Range(0f, 1f)]
    [SerializeField] float fallOffGravityScale;
    [SerializeField] float downFallGravityScale = 1.25f;
    [SerializeField] int jumpsMax = 1;
    int jumpsCount = 0;
    #endregion

    #region Prefabs
    [Header("Meatboy Particles")]
    [SerializeField] ParticleSystem runParticles;
    bool runParticlesPlaying = false;
    [SerializeField] ParticleSystem landParticles;
    #endregion

    #region Respawn
    [Tooltip("Meurs instantanément en dessous de cette position")]
    [SerializeField] float yDeathLimit = -5f;
    Vector3 respawnPosition;

    #endregion

    #region WallSlide
    [Header("WallJump & Sliding")]
    [SerializeField] WallSideChecker wallSideChecker;
    bool wallSliding = false;
    bool wallJumping = false;
    [SerializeField] float wallSlidingSpeed = 2f;
    float wallJumpTimer = 0f;
    float wallJumpMaxTime;
    [SerializeField] float walledYJumpSpeed;
    [Tooltip("Intensité et durée de la vélocité en X lors du walljump")]
    [SerializeField] AnimationCurve wallJumpXCurve;
    int wallJumpDir = 1;

    #endregion
    protected override void Awake()
    {
        base.Awake();
        groundChecker.OnGrounded += Landing;
        groundChecker.OnGrounded += ResetJumpCount;
        groundChecker.OnGrounded += wallSideChecker.ResetLastWall;
        groundChecker.OnGrounded += ResetWallJump;
        wallSideChecker.onNewWalled += ResetJumpCount;
    }

    private void Start()
    {
        respawnPosition = transform.position;
        wallJumpMaxTime = wallJumpXCurve.keys[wallJumpXCurve.length - 1].time;
        wallJumpTimer = wallJumpMaxTime;
    }

    bool shouldJump = false;

    protected override bool isGrounded { get { return groundChecker.IsGrounded; } }
    protected override bool isWalled { get { return wallSideChecker.IsWalled; } }

    void Update()
    {
        if (!isDead)
        {
            mouvement.x = Input.GetAxis("Horizontal");

            if (!isGrounded)
                rb2D.velocity -= new Vector2(0, 9.81f * Time.deltaTime * rb2D.gravityScale);
            //Look if we can jump
            if ((groundChecker.IsGrounded || wallSideChecker.IsWalled) && Input.GetButtonDown("Jump") && jumpsCount < jumpsMax)
            {
                shouldJump = true;

                if (wallSliding)
                {
                    wallJumping = true;
                    wallJumpDir = lookingDirection;
                    //Start timer
                    wallJumpTimer = 0f;
                }
            }
            //If we shouldn't jump and already in the air, pressing "Jump", slow down gravityScale to reduce falling speed
            #region Slow Fall
            else if (!groundChecker.IsGrounded && Input.GetButton("Jump"))
                rb2D.gravityScale = fallOffGravityScale;

            else if (!groundChecker.IsGrounded && Input.GetAxis("Vertical") < 0f)
            {
                rb2D.gravityScale = Mathf.Lerp(1.0f, downFallGravityScale, Mathf.Abs(Input.GetAxis("Vertical")));
            }
            else
                rb2D.gravityScale = 1.0f;

            #endregion

            if (wallJumping)
            {
                if (wallJumpTimer < wallJumpMaxTime)
                {
                    wallJumpTimer += Time.deltaTime;

                }
                else
                {
                    wallJumpTimer = wallJumpMaxTime;
                    wallJumping = false;
                }
            }

            if ((int)Input.GetAxisRaw("Horizontal") != 0f && (int)Input.GetAxisRaw("Horizontal") != lookingDirection)
                Turn((int)Input.GetAxisRaw("Horizontal"));

            #region Run Particles
            //If particles aren't playing and there is movement, play the animation
            if (!runParticlesPlaying && mouvement != Vector2.zero)
            {
                runParticles.Play();
                runParticlesPlaying = true;
            }
            //Otherwise, stop it
            else if (runParticlesPlaying && mouvement == Vector2.zero)
            {
                runParticles.Stop();
                runParticlesPlaying = false;
            }
            #endregion

            if (!isGrounded && isWalled && mouvement.x != 0f)
                wallSliding = true;

            else
                wallSliding = false;
        }

        if (transform.position.y < yDeathLimit && !isDead)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (wallSliding)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (!wallSideChecker.IsWalled)
            Move(mouvement.x);
        else if (wallJumping)
            Move(mouvement.x);

        if (shouldJump)
        {
            Jump();
            shouldJump = false;
        }
    }

    private void LateUpdate()
    {
        //Animation
        animator.SetFloat("HorizontalMovement", Mathf.Lerp(0f, 1f, Mathf.Abs(rb2D.velocity.x)));
        animator.SetBool("InTheAir", !groundChecker.IsGrounded);
        if (!groundChecker.IsGrounded)
            animator.SetFloat("VerticalMovement", rb2D.velocity.y);

        //Run Particles
        ParticleSystem.ShapeModule runParticlesShape = runParticles.shape;
        runParticlesShape.rotation = new Vector3(lookingDirection * Mathf.Abs(runParticlesShape.rotation.x), runParticlesShape.rotation.y, runParticlesShape.rotation.z);
    }

    void ResetJumpCount()
    {
        jumpsCount = 0;
    }

    public override void Die()
    {
        OnDie?.Invoke();
        mouvement = Vector2.zero;
        spriteRenderer.enabled = false;
        col2D.enabled = false;
        isDead = true;
    }

    public void Respawn()
    {
        transform.position = respawnPosition;
        isDead = false;
        spriteRenderer.enabled = true;
        col2D.enabled = true;
    }

    public override void Jump()
    {
        if (!wallJumping)
            rb2D.velocity += Vector2.up * jumpSpeed * Time.deltaTime;

        else
        {
            Turn(-lookingDirection);
            rb2D.velocity += Vector2.up * walledYJumpSpeed * Time.deltaTime;
        }

        jumpsCount++;
    }

    public override void Move(float horizontalAxis)
    {
        //Move the character by its rigidbody velocity
        Vector2 velocity = rb2D.velocity;
        velocity.x = horizontalAxis * speed * Time.deltaTime;
        //If we walljump, change the way the velocity is added
        if (wallJumping)
        {
            velocity.x += -wallJumpDir * wallJumpXCurve.Evaluate(wallJumpTimer);

        }

        //velocity.x = Mathf.Clamp(velocity.x, -speed * Time.deltaTime, speed * Time.deltaTime);
        rb2D.velocity = velocity;
        rb2D.velocity = new Vector2(Mathf.Clamp(rb2D.velocity.x, -speed * Time.deltaTime, speed * Time.deltaTime), rb2D.velocity.y);

    }

    void Turn(int direction)
    {
        //Turn the character depending on where he is going
        if (direction > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            wallSideChecker.SetSideToCheck(Side.Right);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            wallSideChecker.SetSideToCheck(Side.Left);
        }
        lookingDirection = direction;
    }

    void Landing()
    {
        landParticles.Play();
        if (wallJumping)
            wallJumping = false;
    }

    void ResetWallJump()
    {
        wallJumpTimer = wallJumpMaxTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap" && !isDead)
        {
            Debug.Log("die");
            Die();
        }
    }
}