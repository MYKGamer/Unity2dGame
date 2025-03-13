using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Controller : MonoBehaviour
{
    // Particle System References
    public ParticleSystem movementParticle;
    public ParticleSystem fallParticle;
    public ParticleSystem leftTouchParticle;
    public ParticleSystem rightTouchParticle;
    public ParticleSystem dieParticle;  // ✅ Added DieParticle

    // Ground Detection
    public bool isGrounded = false;

    public Rigidbody2D rb;

    // Movement Threshold for triggering particles
    public float movementThreshold = 0.1f;

    public BoxCollider2D leftTouchCollider;
    public BoxCollider2D rightTouchCollider;

    private bool leftParticleHasPlayed = false;
    private bool rightParticleHasPlayed = false;

    void Awake()
    {
        if (rb == null && transform.parent != null)
        {
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // Movement Particle Logic
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

        // Fall Particle Logic
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
    }

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

    // ✅ Function to play Die Particle
    public void PlayDieParticle(Vector3 position)
    {
        if (dieParticle != null)
        {
            dieParticle.transform.position = position;
            dieParticle.Play();
        }
    }
}
