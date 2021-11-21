using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpSpeed;
    public float gravity;
    [SerializeField] private BoxCollider2D topCollider;
    [SerializeField] private CircleCollider2D bottomCollider;

    [Header("Ground and Ceiling Settings")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance;
    public bool isGrounded;
    public Transform ceilingCheck;

    [Header("Others")]
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isCrouching;
    [SerializeField] private Animator levelNotifierAnimator;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    [Header("Score")]
    [SerializeField] private IntValueSO scoreValue;
    
    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO firstInputSignal;
    [SerializeField] private VoidSignalSO fadeOutSignal;

    [Header("Signals Listening On")]
    [SerializeField] private IntSignalSO coinCollectedSignal;
    [SerializeField] private VoidSignalSO restartLevelSignal;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    Vector3 initialPosition;
    Vector3 lastCheckpointPosition;
    Vector3 lastGroundedPosition;
    float moveInput;
    Vector2 velocity;
    Vector2 previousVelocity;
    RaycastHit2D hit;
    bool firstInputRaised = false;


    private void Start() 
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        initialPosition = transform.position;
        lastCheckpointPosition = initialPosition;
        
        fadeOutSignal.RaiseSignal();
        inputReader.EnablePlayerInput();
        levelNotifierAnimator.enabled = true;
        Application.targetFrameRate = 30;
    }

    private void OnEnable()
    {
        inputReader.moveEvent += OnMove;
        inputReader.jumpEvent += OnJump;
        inputReader.jumpCanceledEvent += OnJumpCancel;
        inputReader.crouchEvent += OnCrouch;
        inputReader.crouchCanceledEvent += OnCrouchCancel;

        coinCollectedSignal.OnSignalRaised += CoinCollected;
        restartLevelSignal.OnSignalRaised += RestartLevel;
    }

    private void OnDisable()
    {
        inputReader.moveEvent -= OnMove;
        inputReader.jumpEvent -= OnJump;
        inputReader.jumpCanceledEvent -= OnJumpCancel;
        inputReader.crouchEvent -= OnCrouch;
        inputReader.crouchCanceledEvent -= OnCrouchCancel;

        coinCollectedSignal.OnSignalRaised -= CoinCollected;
        restartLevelSignal.OnSignalRaised -= RestartLevel;
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        RecalculateMovement();
    }

    private void CheckGrounded()
    {
        hit = Physics2D.CircleCast(groundCheck.position, groundDistance, Vector2.down, groundDistance, groundMask);

        isGrounded = hit.collider == null ? false : true;
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
            lastGroundedPosition = transform.position;
        }
    }

    private void RecalculateMovement()
    {
        // On Ground
        if (isGrounded)
        {
            // Movement
            velocity.x = moveInput * moveSpeed;
            if (moveInput == -1f)
                spriteRenderer.flipX = true;
            if (moveInput == 1f)
                spriteRenderer.flipX = false;


            if (Physics2D.CircleCast(ceilingCheck.position, groundDistance, Vector2.up, groundDistance, groundMask).collider != null)
            {
                isCrouching = true;
                animator.SetBool("isCrouching", isCrouching);
            }

            // Jump only while not crouching
            if (isJumping && !isCrouching)
            {
                velocity.y = jumpSpeed;
            }

            // Crouch at half speed
            if (isCrouching)
            {
                velocity.x /= 2;
                topCollider.enabled = false;
                bottomCollider.offset = new Vector2(0.1635132f, -0.78f);
                bottomCollider.radius = 0.68f;
            } else {
                topCollider.enabled = true;
                bottomCollider.offset = new Vector2(0.1635132f, -0.22f);
                bottomCollider.radius = 1.36f;
            }
        
        // In Air    
        } else {

            // If not grounded, pressing the crouch key will make the player fall down
            if (isCrouching)
            {
                isJumping = false;
                isCrouching = false;
                
                previousVelocity.x = 0;
                velocity.x = 0;
                previousVelocity.y = 0;
                velocity.y = 0;

                velocity.y += gravity;
            }

            // Free Fall
            velocity.y += gravity;
        }

        velocity.x = Mathf.Lerp(previousVelocity.x, velocity.x, Time.fixedDeltaTime * 4f);
        velocity.y = Mathf.Lerp(previousVelocity.y, velocity.y, Time.fixedDeltaTime * 4f);

        if (moveInput == 0f)
        {
            velocity.x = Mathf.Lerp(previousVelocity.x, velocity.x, Time.fixedDeltaTime * 100f);
            velocity.y = Mathf.Lerp(previousVelocity.y, velocity.y, Time.fixedDeltaTime * 100f);
        }

        velocity.x = Mathf.Clamp(velocity.x, -(moveSpeed - 0.1f), moveSpeed - 0.1f);
        velocity.y = Mathf.Clamp(velocity.y, -50f, 100f);

        rigidBody.velocity = velocity;

        previousVelocity = velocity;
    }

    private void OnMove(float move)
    {
        if (!firstInputRaised)
        {
            firstInputSignal.RaiseSignal();
            //Debug.Log("PlayerController: First Input Signal Raised", gameObject);
            firstInputRaised = true;
        }

        animator.SetBool("isMoving", move != 0); //&& isGrounded && !isCrouching);
        
        moveInput = move;
    }

    private void OnJump()
    {
        isJumping = true;
    }

    private void OnJumpCancel()
    {
        isJumping = false;
    }

    private void OnCrouch()
    {
        isCrouching = true;
        animator.SetBool("isCrouching", isCrouching);
    }

    private void OnCrouchCancel()
    {
        isCrouching = false;
        animator.SetBool("isCrouching", isCrouching);
    }

    private void CoinCollected(int points)
    {
        scoreValue.Increment(points);
    }

    private void ResetPosition()
    {
        transform.position = lastCheckpointPosition;
        // transform.position = lastGroundedPosition;
        spriteRenderer.flipX = false;
    }

    public void ResetVelocity()
    {
        previousVelocity = Vector2.zero;
        velocity = Vector2.zero;
    }

    public void EnableColliders()
    {
        topCollider.enabled = true;
        bottomCollider.enabled = true;
    }

    public void DisableColliders()
    {
        topCollider.enabled = false;
        bottomCollider.enabled = false;
    }

    private void RestartLevel()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isGrounded", false);
        animator.SetBool("isCrouching", false);
        animator.SetBool("killed", false);
        
        ResetVelocity();
        EnableColliders();
        ResetPosition();

        firstInputRaised = false;
    }

    public void SetLastCheckpoint(Vector3 checkpoint)
    {
        lastCheckpointPosition = checkpoint;
    }

    public void Kill()
    {
        animator.SetBool("killed", true);
        ResetVelocity();
    }
}
