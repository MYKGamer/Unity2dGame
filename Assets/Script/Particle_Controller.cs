using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Controller : MonoBehaviour
{
    // Particle systems ke references:
    public ParticleSystem movementParticle;
    public ParticleSystem fallParticle;
    public ParticleSystem touchParticle;
    
    // Booleans, collision events se update honge:
    public bool isGrounded = false;
    public bool isWallTouch = false;
    
    // Player ke Rigidbody2D ka reference.
    // Agar manually assign na ho to Awake() me parent (Player) se assign ho jayega.
    public Rigidbody2D rb;
    
    // Horizontal movement threshold.
    public float movementThreshold = 0.1f;
    
    // References to the two BoxCollider2D components on this Particle GameObject:
    public BoxCollider2D leftCollider;
    public BoxCollider2D rightCollider;
    
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
        // Movement Particle: Agar player horizontally move kar raha ho.
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
        
        // Fall Particle: Agar player ground par hai.
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
        
        // Touch Particle: Agar player wall touch kar raha ho.
        if (isWallTouch)
        {
            if (!touchParticle.isPlaying)
                touchParticle.Play();
        }
        else
        {
            if (touchParticle.isPlaying)
                touchParticle.Stop();
        }
        
        // Manage the colliders based on horizontal movement:
        if (rb.linearVelocity.x < -movementThreshold)
        {
            // Player moving left: enable leftCollider, disable rightCollider.
            if (leftCollider != null) leftCollider.enabled = true;
            if (rightCollider != null) rightCollider.enabled = false;
        }
        else if (rb.linearVelocity.x > movementThreshold)
        {
            // Player moving right: enable rightCollider, disable leftCollider.
            if (leftCollider != null) leftCollider.enabled = false;
            if (rightCollider != null) rightCollider.enabled = true;
        }
        else
        {
            // No horizontal movement: enable both colliders.
            if (leftCollider != null) leftCollider.enabled = true;
            if (rightCollider != null) rightCollider.enabled = true;
        }
    }
    
    // Collision triggers: Ground aur Wall tag ke liye.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.CompareTag("Wall"))
        {
            isWallTouch = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.CompareTag("Wall"))
        {
            isWallTouch = false;
        }
    }
}
