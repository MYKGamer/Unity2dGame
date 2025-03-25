using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Ensure correct namespace

public class Player_Movement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;  // SpriteRenderer reference for flipping

    public float speed = 5f;
    public float smoothTime = 0.1f; // Smoothness factor, jitna chota value, utna tez smoothing

    float moveInput;             // Horizontal input value
    float velocityX = 0f;        // Reference velocity for SmoothDamp

    // Platform related variables
    public bool IsOnPlatform;
    public Rigidbody2D PlatformRB;

    // Jump Controller (Updated to Double Jump) .................................................
    public float jumpForce = 10f; // Jump power
    public LayerMask groundLayer; // Ground layer
    public Transform groundCheck; // Ground check position
    public float groundCheckRadius = 0.2f; // Ground detection radius

    private bool isGrounded;
    private bool canDoubleJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer component ko get karo
    }

    void Update()
    {
        float targetSpeed = speed * moveInput;
        float smoothVelocity = Mathf.SmoothDamp(rb.linearVelocity.x, targetSpeed, ref velocityX, smoothTime);
        
        // Agar player platform par hai, to platform ki horizontal velocity add karo.
        if (IsOnPlatform && PlatformRB != null)
        {
            rb.linearVelocity = new Vector2(smoothVelocity + PlatformRB.linearVelocity.x, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(smoothVelocity, rb.linearVelocity.y);
        }

        Flip(); // Flip function call to update sprite direction

        // Updated Jump Code (From PlayerDoubleJump Script) .......................................................
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded) // First jump
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (canDoubleJump) // Double jump
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = false;
            }
        }
    }

    // Unity Events ke liye correct signature:
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            moveInput = inputVector.x; // Sirf horizontal value
        }
        else if (context.canceled)
        {
            moveInput = 0f; // Input cancel hone par zero set karein
        }
    }

    // Flip function: Agar left button press ho to flip enable (true) ho aur right button press par flip disable (false)
    void Flip()
    {
        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckRadius);
        }
    }
}
