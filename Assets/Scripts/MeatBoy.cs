using UnityEngine;

public class MeatBoy : Character
{
    #region Sauts
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
    Vector3 respawnPosition;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        groundChecker.OnGrounded += Landing;
    }

    private void Start()
    {
        respawnPosition = transform.position;
    }

    bool shouldJump = false;

    void Update()
    {
        mouvement.x = Input.GetAxis("Horizontal");

        //Look if we can and should jump
        if (groundChecker.IsGrounded && Input.GetButtonDown("Jump") && jumpsCount < jumpsMax)
        {
            shouldJump = true;
        }
        //If we shouldn't jump and already in the air, pressing "Jump", slow down gravityScale to reduce falling speed
        else if (!groundChecker.IsGrounded && Input.GetButton("Jump"))
        {
            rb2D.gravityScale = fallOffGravityScale;
        }
        else if (!groundChecker.IsGrounded && Input.GetAxis("Vertical") < 0f)
        {
            Debug.Log("DOWN");
            rb2D.gravityScale = Mathf.Lerp(1.0f, downFallGravityScale, Mathf.Abs(Input.GetAxis("Vertical")));
        }
        else
        {
            rb2D.gravityScale = 1.0f;
            if (jumpsCount > 0)
                jumpsCount = 0;
        }

        Turn((int)Input.GetAxisRaw("Horizontal"));

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
    }

    private void FixedUpdate()
    {
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

    public override void Die()
    {
        rb2D.isKinematic = true;
        transform.position = respawnPosition;
        rb2D.isKinematic = false;

        mouvement = Vector2.zero;
    }

    public override void Jump()
    {
        rb2D.velocity = Vector2.up * jumpSpeed * Time.deltaTime;
        jumpsCount++;
    }

    public override void Move(float horizontalAxis)
    {
        //Move the character by its rigidbody velocity
        Vector2 velocity = rb2D.velocity;
        velocity.x = horizontalAxis * speed * Time.deltaTime;
        rb2D.velocity = velocity;
    }

    void Turn(int direction)
    {
        if (direction != lookingDirection && direction != 0)
        {
            //Turn the character depending on where he is going
            if (direction > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (direction < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            lookingDirection = direction;
        }
    }

    void Landing()
    {
        //landParticles.Play();
    }
}