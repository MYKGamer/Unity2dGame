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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer component ko get karo
    }

    void Update()
    {
        float targetSpeed = speed * moveInput;
        // SmoothDamp se current velocity ko smoothly targetSpeed ke taraf adjust karte hain
        float smoothVelocity = Mathf.SmoothDamp(rb.velocity.x, targetSpeed, ref velocityX, smoothTime);
        rb.velocity = new Vector2(smoothVelocity, rb.velocity.y);

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

    // Flip function: Agar left button press ho to flip enable (true) ho aur right button press par disable (false)
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
