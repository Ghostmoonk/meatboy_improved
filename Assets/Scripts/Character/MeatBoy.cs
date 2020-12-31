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
    [SerializeField] float airControl = 10f;
    #endregion

    #region Prefabs
    [Header("Meatboy Particles")]
    [SerializeField] ParticleSystem runParticles;
    [SerializeField] ParticleSystem deathParticles;
    bool runParticlesPlaying = false;
    [SerializeField] ParticleSystem landParticles;
    #endregion

    #region Respawn
    [Tooltip("Meurt instantanément en dessous de cette position")]
    [SerializeField] float yDeathLimit = -5f;
    Vector3 respawnPosition;

    #endregion

    #region WallSlide
    [Header("WallJump & Sliding")]
    [SerializeField] WallSideChecker wallSideChecker;
    bool wallSliding = false;
    [SerializeField] float wallSlidingSpeed = 2f;
    float wallJumpTimer = 0f;
    float wallJumpMaxTime;
    [SerializeField] float walledYJumpSpeed;
    int wallJumpDir = 1;
    [SerializeField] Vector2 wallJumpForce;
    #endregion

    #region Audio
    [Header("Audio")]
    [SerializeField] AudioSource footAudioSource;
    [SerializeField] AudioSource bodyAudioSource;
    [SerializeField] string footstepSoundName;
    [SerializeField] string jumpSoundName;
    [SerializeField] string landingSoundName;
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
        wallJumpTimer = wallJumpMaxTime;
    }

    bool shouldJump = false;

    protected override bool isGrounded { get { return groundChecker.IsGrounded; } }
    protected override bool isWalled { get { return wallSideChecker.IsWalled; } }

    void Update()
    {
        if (!isDead && canMove)
        {
            mouvement.x = Input.GetAxis("Horizontal");

            //Look if we can jump
            if ((groundChecker.IsGrounded || wallSideChecker.IsWalled) && Input.GetButtonDown("Jump") && jumpsCount < jumpsMax)
                shouldJump = true;

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

            if ((int)Input.GetAxisRaw("Horizontal") != 0f && (int)Input.GetAxisRaw("Horizontal") != lookingDirection)
                Turn((int)Input.GetAxisRaw("Horizontal"));

            #region Run Particles
            //If particles aren't playing and there is movement, play the animation
            if (!runParticlesPlaying && rb2D.velocity != Vector2.zero)
            {
                runParticles.Play();
                runParticlesPlaying = true;
            }
            //Otherwise, stop it
            else if (runParticlesPlaying && rb2D.velocity == Vector2.zero)
            {
                runParticles.Stop();
                runParticlesPlaying = false;
            }
            #endregion

            if (!isGrounded && isWalled && (wallSideChecker.GetSideWalled() == Side.Right && mouvement.x > 0f || wallSideChecker.GetSideWalled() == Side.Left && mouvement.x < 0f))
                wallSliding = true;

            else
                wallSliding = false;
        }
        else if (mouvement != Vector2.zero)
        {
            mouvement = Vector2.zero;
        }

        if (transform.position.y < yDeathLimit && !isDead)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (wallSliding)
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlidingSpeed, float.MaxValue));

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

    void ResetJumpCount() => jumpsCount = 0;

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
        ResetVelocity();
        isDead = false;
        spriteRenderer.enabled = true;
        col2D.enabled = true;
        deathParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        runParticles.Clear();
    }

    public override void Jump()
    {
        if (isGrounded)
            //rb2D.velocity += Vector2.up * jumpSpeed * Time.deltaTime;
            rb2D.AddForce(Vector2.up * jumpSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);

        else
        {
            Turn(-lookingDirection);
            //rb2D.AddForce(new Vector2(3500f, 1500f) * Time.fixedDeltaTime);
            Debug.Log(wallSideChecker.GetSideWalled());
            Vector2 direction = wallSideChecker.GetSideWalled() == Side.Right ? new Vector2(-1f, 1f) : new Vector2(1f, 1f);
            direction = direction.normalized;
            Debug.Log("Force ajoutée : " + direction * jumpSpeed * Time.fixedDeltaTime);
            rb2D.AddForce(direction * jumpSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }

        PlayFootSound(jumpSoundName);
        jumpsCount++;
    }

    public override void Move(float horizontalAxis)
    {
        Vector2 velocity = Vector2.zero;
        velocity.x = horizontalAxis * airControl * Time.deltaTime;

        velocity.x = Mathf.Clamp(velocity.x, -speed * Time.deltaTime, airControl * Time.deltaTime);
        rb2D.velocity += velocity;

        if (isGrounded)
            rb2D.velocity = new Vector2(horizontalAxis * speed * Time.deltaTime, rb2D.velocity.y);
        //rb2D.velocity = new Vector2(Mathf.Clamp(rb2D.velocity.x, -speed * Time.deltaTime, speed * Time.deltaTime), rb2D.velocity.y);

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
    }

    void ResetWallJump()
    {
        wallJumpTimer = wallJumpMaxTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap" && !isDead)
        {
            Die();
        }
        if (collision.GetComponent<MonoBehaviour>() is ICollectable)
        {
            ICollectable collectable = (ICollectable)collision.GetComponent<MonoBehaviour>();
            if (!collectable.GetIsCollected())
                collectable.Collect();
        }
    }

    public void SetCanMove(bool toggle) => canMove = toggle;

    public void SetCheckpointPosition(Vector3 newCheckpointPosition) => respawnPosition = newCheckpointPosition;

    public void ResetVelocity() => rb2D.velocity = Vector2.zero;

    #region Sound


    public void PlayFootSound(string soundName)
    {
        SoundManager.Instance.PlaySound(footAudioSource, soundName);
    }

    public void PlayBodySound(string soundName)
    {
        SoundManager.Instance.PlaySound(bodyAudioSource, soundName);
    }

    #endregion
}