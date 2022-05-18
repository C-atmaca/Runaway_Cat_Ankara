using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public event Action onPlayerFallDamage;

    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider;

    [SerializeField] private ParticleSystem dust;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask climbWallMask;
    [Header("Movement Speed")]
    [SerializeField] private float movementSpeed = 2f;
    [Header("Jump & Gravity")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpTime; // Maximum time the player is allowed to keep going up after jumping
    [SerializeField] private float gravityNormal = 10f;
    [SerializeField] private float gravityModifier = 20f; // Gravity when falling down
    private float jumpTimeCounter; // Counter to keep track of the time player spent in air

    private bool isIdle;
    private bool isNotJumping;
    private bool isJumping;
    private bool isRunning;

    private bool isTouchingfront = false;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isWallClimbing;

    private float checkRadius = 0.2f;
    [Header("Wall Climb & Wall Jump")]
    [SerializeField] private Transform frontCheck;
    [SerializeField] private float wallStuckSpeed;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallClimbingSpeed;
    [SerializeField] private float xwallForce;
    [SerializeField] private float wallJumpTime;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float coyoteTimeCounterWall;

    [Header("Player Health Script")]
    private float maxYval;
    [SerializeField] private PlayerHealth playerHealth;
    private int fallDamage = -1;
    [Header("Timer")]
    [SerializeField] TimerController timerController;

    [Header("Audio")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip jump;

    private void Awake()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider = transform.GetComponent<BoxCollider2D>();
        isIdle = true;
    }

    private void Start()
    {
        timerController.BeginTimer();
    }

    private void Update()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            if (maxYval <= -75)
            {
                Shake();
                playerHealth.ChangeHealth(fallDamage);
                if (onPlayerFallDamage != null)
                {
                    onPlayerFallDamage();
                }
                maxYval = 0;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (rigidbody2D.velocity.y < maxYval)
            {
                maxYval = rigidbody2D.velocity.y;
            }
        }
        Movement();
        WallClimbAndJump();
        Jump();
    }

    private void Movement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        if (!isJumping && !isNotJumping && inputX != 0 && IsGrounded())
        {
            isRunning = true;
            isIdle = false;
        }
        else if (IsGrounded() && inputX == 0)
        {
            isIdle = true;
            isRunning = false;
        }
        Flip(inputX);
        rigidbody2D.velocity = new Vector2(inputX * movementSpeed, rigidbody2D.velocity.y); // Move according to the input by manipulating velocity
    }

    private void WallClimbAndJump()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        isTouchingfront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, climbWallMask);
        if (isTouchingfront)
        {
            coyoteTimeCounterWall = coyoteTime;
        }
        else
        {
            coyoteTimeCounterWall -= Time.deltaTime;
        }

        if (isTouchingfront && !IsGrounded())
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            //Mathf.Clamp(rigidbody2D.velocity.y, -wallSlidingSpeed, float.MaxValue)
            if (Input.GetKey(KeyCode.S))
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Clamp(rigidbody2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Clamp(rigidbody2D.velocity.y, wallStuckSpeed, float.MaxValue));
            }
        }

        if (Input.GetKey(KeyCode.W) && isWallSliding)
        {
            isWallClimbing = true;
        }
        else
        {
            isWallClimbing = false;
        }
        if (isWallClimbing)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, wallClimbingSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounterWall > 0)
        {
            source.PlayOneShot(jump);
            isWallJumping = true;
            maxYval = 0;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }
        if (isWallJumping)
        {
            //Stretch();

            rigidbody2D.AddForce(new Vector2(xwallForce * inputX, 0), ForceMode2D.Impulse);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
        }
    }

    private void Jump()
    {
        bool jumpInput = Input.GetButtonDown("Jump"); // Get button down once
        bool jumpInputAir = Input.GetButton("Jump"); // Get button hold
        bool jumpInputReleased = Input.GetButtonUp("Jump"); // Get button up once
        if (jumpInput && coyoteTimeCounter > 0)
        {
            source.PlayOneShot(jump);
            CreateDust();
            maxYval = 0;
            isJumping = true;
            jumpTimeCounter = jumpTime; // Set the counter
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce); // Jump by manipulating velocity
        }
        if (jumpInputAir && isJumping && !IsHitHead())
        {
            if (jumpTimeCounter > 0)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce - jumpForce / 4); // Keep moving up if the timer is not done
                jumpTimeCounter -= Time.deltaTime; // Timer working
            }
            else
            {
                isJumping = false;
                isNotJumping = true;
            }
        }
        if (jumpInputReleased || rigidbody2D.velocity.y < 0)
        {
            isJumping = false;
            isNotJumping = true;
            rigidbody2D.gravityScale = gravityModifier; // Change gravity while falling down
            coyoteTimeCounter = 0;
        }
        else
        {
            rigidbody2D.gravityScale = gravityNormal; // Change gravity back to normal after falling
        }
    }

    private bool IsGrounded() // Ground Check
    {
        float extraHeightText = .1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeightText, groundLayerMask);
        return raycastHit.collider != null;
    }

    private bool IsHitHead() // Above Ground Check
    {
        float extraHeightText = .05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, extraHeightText, groundLayerMask);
        return raycastHit.collider != null;
    }

    private void Flip(float inputX)
    {
        if (inputX > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            CreateDust();
        }
        else if (inputX < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            CreateDust();
        }
    }

    private void SetWallJumpingToFalse()
    {
        isWallJumping = false;
        coyoteTimeCounterWall = 0;
    }

    private void Shake()
    {
        const float duration = 0.1f;
        const float strength = 0.8f;

        var tween = transform.DOShakeScale(duration, strength);
        if (tween.IsPlaying())
        {
            return;
        }
        else
        {
            transform.DOScaleY(1, duration * 2);
        }
        
    }

    public void ActivateGrapple()
    {
        GetComponent<Grappler>().enabled = true;
    }

    private void CreateDust()
    {
        dust.Play();
    }

}
