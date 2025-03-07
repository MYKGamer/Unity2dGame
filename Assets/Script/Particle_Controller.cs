using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Controller : MonoBehaviour
{
    // ---------------------------------------------------------------------
    // Particle System References
    // ---------------------------------------------------------------------
    public ParticleSystem movementParticle;
    public ParticleSystem fallParticle;
    public ParticleSystem leftTouchParticle;
    public ParticleSystem rightTouchParticle;
    
    // ---------------------------------------------------------------------
    // Ground Detection
    // ---------------------------------------------------------------------
    public bool isGrounded = false;
    
    // ---------------------------------------------------------------------
    // Player Rigidbody Reference
    // If not manually assigned, this will be set from the parent (Player) in Awake()
    // ---------------------------------------------------------------------
    public Rigidbody2D rb;
    
    // ---------------------------------------------------------------------
    // Movement Threshold for triggering particles
    // ---------------------------------------------------------------------
    public float movementThreshold = 0.1f;
    
    // ---------------------------------------------------------------------
    // Touch Collider References
    // These are the two BoxCollider2D components on this Particle GameObject
    // ---------------------------------------------------------------------
    public BoxCollider2D leftTouchCollider;
    public BoxCollider2D rightTouchCollider;
    
    // ---------------------------------------------------------------------
    // Persistent Flags to ensure each touch particle plays only once per collision event
    // ---------------------------------------------------------------------
    private bool leftParticleHasPlayed = false;
    private bool rightParticleHasPlayed = false;
    
    // ---------------------------------------------------------------------
    // Awake: Auto-assign Rigidbody2D from parent if not set manually
    // ---------------------------------------------------------------------
    void Awake()
    {
        if (rb == null && transform.parent != null)
        {
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }
    }
    
    // ---------------------------------------------------------------------
    // Update: Main Loop for Particle Control
    // ---------------------------------------------------------------------
    void Update()
    {
        // --- Movement Particle Logic ---
        if (Mathf.Abs(rb.linearVelocity.x) > movementThreshold)
        {
            if (!movementParticle.isPlaying)
                movementParticle.Play();
        }
        else
        {
            if (movementParticle.isPlaying)
                movementParticle.Stop();
        }
        
        // --- Fall Particle Logic ---
        if (isGrounded)
        {
            if (!fallParticle.isPlaying)
                fallParticle.Play();
                isGrounded = false;  // Reset flag after playing
        }
        else
        {
            if (fallParticle.isPlaying)
                fallParticle.Stop();
        }
        
        // --- Manage Touch Colliders Based on Horizontal Movement ---
        if (rb.linearVelocity.x < -movementThreshold)
        {
            // Player moving left: enable left collider, disable right collider.
            if (leftTouchCollider != null) leftTouchCollider.enabled = true;
            if (rightTouchCollider != null) rightTouchCollider.enabled = false;
        }
        else if (rb.linearVelocity.x > movementThreshold)
        {
            // Player moving right: enable right collider, disable left collider.
            if (leftTouchCollider != null) leftTouchCollider.enabled = false;
            if (rightTouchCollider != null) rightTouchCollider.enabled = true;
        }
        else
        {
            // No horizontal movement: enable both colliders.
            if (leftTouchCollider != null) leftTouchCollider.enabled = true;
            if (rightTouchCollider != null) rightTouchCollider.enabled = true;
        }
        
        // --- Check Overlaps for Left and Right Touch Colliders ---
        bool leftTouchDetected = false;
        bool rightTouchDetected = false;
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.useLayerMask = false;
        Collider2D[] results = new Collider2D[10];
        
        // Left touch collider check (trigger with "Diwar" tag)
        if (leftTouchCollider != null && leftTouchCollider.enabled)
        {
            int leftCount = leftTouchCollider.Overlap(filter, results);
            for (int i = 0; i < leftCount; i++)
            {
                if (results[i].CompareTag("Diwar"))
                {
                    leftTouchDetected = true;
                    break;
                }
            }
        }
        
        // Right touch collider check (trigger with "Wall" tag)
        if (rightTouchCollider != null && rightTouchCollider.enabled)
        {
            int rightCount = rightTouchCollider.Overlap(filter, results);
            for (int i = 0; i < rightCount; i++)
            {
                if (results[i].CompareTag("Wall"))
                {
                    rightTouchDetected = true;
                    break;
                }
            }
        }
        
        // --- Control Left Touch Particle ---
        if (leftTouchDetected)
        {
            if (!leftParticleHasPlayed)
            {
                if (!leftTouchParticle.isPlaying)
                    leftTouchParticle.Play();
                leftParticleHasPlayed = true;
            }
        }
        else
        {
            if (leftTouchParticle.isPlaying)
                leftTouchParticle.Stop();
            leftParticleHasPlayed = false;
        }
        
        // --- Control Right Touch Particle ---
        if (rightTouchDetected)
        {
            if (!rightParticleHasPlayed)
            {
                if (!rightTouchParticle.isPlaying)
                    rightTouchParticle.Play();
                rightParticleHasPlayed = true;
            }
        }
        else
        {
            if (rightTouchParticle.isPlaying)
                rightTouchParticle.Stop();
            rightParticleHasPlayed = false;
        }
    }
    
    // ---------------------------------------------------------------------
    // Collision Triggers for Ground (affecting Fall Particle)
    // ---------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
