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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer component ko get karo
    }

    void Update()
    {
        float targetSpeed = speed * moveInput;
        float smoothVelocity = Mathf.SmoothDamp(rb.velocity.x, targetSpeed, ref velocityX, smoothTime);
        
        // Agar player platform par hai, to platform ki horizontal velocity add karo.
        if (IsOnPlatform && PlatformRB != null)
        {
            rb.velocity = new Vector2(smoothVelocity + PlatformRB.velocity.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(smoothVelocity, rb.velocity.y);
        }

        Flip(); // Flip function call to update sprite direction
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
}
