using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Controller : MonoBehaviour
{
    // Particle systems ke references:
    public ParticleSystem movementParticle;
    public ParticleSystem fallParticle;
    public ParticleSystem leftTouchParticle;
    public ParticleSystem rightTouchParticle;
    
    // Ground ke liye:
    public bool isGrounded = false;
    
    // Player ke Rigidbody2D ka reference.
    // Agar manually assign na ho to Awake() me parent (Player) se assign ho jayega.
    public Rigidbody2D rb;
    
    // Horizontal movement threshold.
    public float movementThreshold = 0.1f;
    
    // References to the two BoxCollider2D components on this Particle GameObject:
    public BoxCollider2D leftTouchCollider;
    public BoxCollider2D rightTouchCollider;
    
    // Persistent flags to ensure each touch particle plays only once per collision event.
    private bool leftParticleHasPlayed = false;
    private bool rightParticleHasPlayed = false;
    
    void Awake()
    {
        // Agar rb manually assign nahi hua, to parent (Player GameObject) se Rigidbody2D get karo.
        if (rb == null && transform.parent != null)
        {
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }
    }
    
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
    
    // Collision triggers for Ground (for fall particle)
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
