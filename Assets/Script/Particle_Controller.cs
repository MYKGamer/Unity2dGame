using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Controller : MonoBehaviour
{
    // Particle systems ke references:
    public ParticleSystem movementParticle;
    public ParticleSystem fallParticle;
    public ParticleSystem touchParticle;
    
    // Ye booleans, collision events se update honge.
    public bool isGrounded = false;
    public bool isWallTouch = false;
    
    // Player ke Rigidbody2D ka reference.
    // Yeh manually assign bhi kiya ja sakta hai, lekin agar null ho to Awake() me parent se assign ho jayega.
    public Rigidbody2D rb;
    
    // Horizontal movement threshold.
    public float movementThreshold = 0.1f;
    
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
