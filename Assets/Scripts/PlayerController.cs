using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public static Rigidbody2D rb;
    public static BoxCollider2D playerCollider;
    public static float maxHealth = 20f;
    public float health = 20f;
    public Animator ani;
    public bool frozen;

    [Header("SFX")]
    public AudioSource jumpSFX;
    public AudioSource dashSFX;
    public AudioSource walkSFX;

    [Header("Abilities")]
    public bool dashEnabled = false;
    public bool superJumpEnabled = false;
    public bool doubleJumpEnabled = false;
    public bool upDashEnabled = true;

    public void setDash(bool value = true)
    {
        dashEnabled = value;
    }
    public void setUpDash(bool value = true)
    {
        upDashEnabled = value;
    }

    [Header("Movement")]
    public float moveDir;
    public float maxSpeed = 14f;
    public float facingDirection;
    public Vector2 movementVector = new Vector2(0f, 0f);
    public float airMult = 0.5f;
    public float runAccel = 8f;
    public float runReduce = 3f;
    public bool canMove;
    public bool lockFacing = false;

    [Header("Dash")]
    public bool canDash = false;
    public bool isDashing = false;
    public IEnumerator dashCoroutine;
    public float dashDistance = 7f;
    public float dashTime = 0.3f;
    private float _dashTimer = 0f;
    public Vector2 dashDir;
    public const float endDashVelocity = 10f;
    public const float endDashUpMult = 0.55f;
    public float dashCooldown = 0.2f;
    private float dashCooldownTimer;
    private Vector2 _beforeDashVelocity;
    public ParticleSystem dashParticles;

    [Header("Super Jump")]
    public float superJumpMult = 2f;
    public float hyperJumpMult = 3f;
    public float superJumpHeight = 6f;
    public float hyperJumpHeight = 3f;
    private bool _superJumping = false;

    [Header("Jumping")]
    public float jumpHeight = 5f;
    public float reduceJumpMult = 0.5f;
    public float currentDoubleJumps = 2f;
    public float doubleJumps = 1f;
    public ParticleSystem doubleJumpParticles;
    private float _jumpVelocity(float height)
    {
        return Mathf.Sqrt(-2 * (Physics2D.gravity.y * rb.gravityScale) * height);
    }

    [Header("Groundcheck")]
    public bool grounded;
    public float groundCheckCastHeight;
    public RaycastHit2D groundCheckRay;
    public LayerMask groundLayerMask;
    public Transform groundCheckTransform;

    [Header("Controls")]
    public InputActionAsset playerInput;
    private InputActionMap inputs;
    public InputAction move;
    public InputAction jump;
    private InputAction dash;
    public InputAction pause;
    public InputAction save;
    public InputAction load;

    void Start()
    {
        groundCheckTransform = transform.Find("Groundcheck").transform;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        canMove = true;
        health = maxHealth;
        inputs = playerInput.FindActionMap("Player");
        inputs.Enable();
        move = inputs.FindAction("Move");
        jump = inputs.FindAction("Jump");
        jump.canceled += ctx => CutJump();
        dash = inputs.FindAction("Dash");
        pause = inputs.FindAction("Pause");
        save = inputs.FindAction("Create Checkpoint");
        load = inputs.FindAction("Load Checkpoint");
    }

    void Update()
    {
        if (frozen)
        {
            canMove = false;
        }

        TimerVariables();

        grounded = Grounded();
        ani.SetBool("Grounded", grounded);

        if (grounded)
        {
            GroundedState();
        }
        else
        {
            AirbornState();
        }

        if (canDash && dash.triggered && dashEnabled)
        {
            Dash();
        }

        if (canMove)
        {
            Move();
            JumpControl();
        }
        if (Mathf.Abs(rb.velocity.x) <= 3)
        {
            walkSFX.Stop();
            ani.SetBool("Running", false);
        }
    }

    void FixedUpdate()
    {
        FacingControl();
    }

    void TimerVariables()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    void Move()
    {

        float mult = grounded ? 1 : airMult;

        moveDir = move.ReadValue<Vector2>()[0];

        if (moveDir != 0)
        {
            if(!walkSFX.isPlaying && grounded)
            {
                walkSFX.Play();
            }
            ani.SetBool("Running", true);
        }

        movementVector.x = moveDir * maxSpeed;
        movementVector.y = rb.velocity.y;

        if (Mathf.Abs(rb.velocity.x) > maxSpeed && System.Math.Sign(rb.velocity.x) == moveDir)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, movementVector, runReduce * mult * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, movementVector, runAccel * mult * Time.deltaTime);
        }
    }

    void FacingControl()
    {
        if (moveDir != 0 && !lockFacing)
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Clamp(180 * -moveDir, 0, 180), 0);
            facingDirection = Mathf.Lerp(1, -1, transform.rotation.y);
        }
    }

    #region Groundcheck
    bool Grounded()
    {
        groundCheckRay = Physics2D.BoxCast(groundCheckTransform.position, new Vector2(playerCollider.bounds.size.x, groundCheckCastHeight), 0f, Vector2.down, groundCheckCastHeight, groundLayerMask.value);
        if (groundCheckRay.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void GroundedState()
    {
        if (!isDashing && dashCooldownTimer <= 0)
        {
            canDash = true;
        }
    }

    void AirbornState()
    {
        if(walkSFX.isPlaying)
        {
            walkSFX.Stop();
        }
        rb.gravityScale = 5;
        _superJumping = false;
    }

    #endregion

    #region Jump
    void JumpControl()
    {
        if (grounded)
        {
            currentDoubleJumps = doubleJumps;
            if (jump.ReadValue<float>() > 0 && !_superJumping)
            {
                Jump();
            }
        }
        else if (jump.ReadValue<float>() > 0 && currentDoubleJumps > 0f && doubleJumpEnabled)
        {
            DoubleJump();
        }
    }


    void Jump()
    {
        jumpSFX.Play();
        rb.velocity = new Vector2(rb.velocity.x, _jumpVelocity(jumpHeight));
    }

    void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, _jumpVelocity(jumpHeight));
        currentDoubleJumps--;
        doubleJumpParticles.Play();
    }

    void CutJump()
    {
        if (!grounded && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * reduceJumpMult);
        }
    }

    void SuperJump()
    {
        float height = superJumpHeight;
        float mult = superJumpMult;

        if (dashDir.y < 0)
        {
            height = hyperJumpHeight;
            mult = hyperJumpMult;
        }

        _superJumping = true;
        jumpSFX.Play();
        rb.velocity = new Vector2(rb.velocity.x * mult, _jumpVelocity(height));
    }

    #endregion

    #region Dash

    void Dash()
    {
        dashDir = move.ReadValue<Vector2>();
        if ((!upDashEnabled || dashDir.x == 0) && dashDir.y == 1)
        {
            if (dashDir.x != 0)
            {
                dashDir.y = 0;
            }
            else
            {
                dashDir.x = facingDirection;
            }
        }
        canDash = false;
        canMove = false;
        isDashing = true;
        rb.gravityScale = 0;
        if (dashDir.normalized == Vector2.zero)
        {
            dashDir.x = facingDirection;
        }
        dashDir = dashDir.normalized;
        _beforeDashVelocity = rb.velocity;
        _dashTimer = dashTime;
        dashCoroutine = DashCoroutine();
        ani.SetBool("Dashing", true);
        dashParticles.Play();
        dashSFX.Play();
        ParticleSystem.ShapeModule particleShape = dashParticles.shape;
        particleShape.rotation = new Vector3(particleShape.rotation.x, particleShape.rotation.y, Vector2.Angle(Vector2.up, -dashDir));
        StartCoroutine(dashCoroutine);
    }

    void DashUpdate()
    {
        float dashSpeed = dashDistance / dashTime;
        Vector2 dashVelocity = dashDir * dashSpeed;
        if (System.Math.Sign(dashVelocity.x) == System.Math.Sign(_beforeDashVelocity.x) && Mathf.Abs(_beforeDashVelocity.x) > Mathf.Abs(dashVelocity.x))
        {
            dashVelocity.x = _beforeDashVelocity.x;
        }
        rb.velocity = dashVelocity;

        if (jump.triggered && grounded && superJumpEnabled)
        {
            StopCoroutine(dashCoroutine);
            DashEnd();
            SuperJump();
        }
    }

    void DashEnd()
    {
        canMove = true;
        isDashing = false;
        ani.SetBool("Dashing", false);
        dashCooldownTimer = dashCooldown;
        rb.gravityScale = 5;
        dashParticles.Stop();
    }

    IEnumerator DashCoroutine()
    {
        while (_dashTimer > 0)
        {
            _dashTimer -= Time.deltaTime;
            DashUpdate();
            yield return null;
        }
        if (dashDir.y <= 0)
        {
            rb.velocity = dashDir * endDashVelocity;
        }
        else if (dashDir.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * endDashUpMult);
        }
        DashEnd();
    }

    #endregion

    public void Freeze()
    {
        StopAllCoroutines();
        DashEnd();
        frozen = true;
        rb.velocity = Vector2.zero;
        dash.Disable();
    }

    public void unFreeze()
    {
        frozen = false;
        canMove = true;
        dash.Enable();
    }

}
