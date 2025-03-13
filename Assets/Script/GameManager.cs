using UnityEngine;
using System.Collections;  // Missing namespace for IEnumerator

public class GameManager : MonoBehaviour
{
    Vector2 CheckPoint;
    Rigidbody2D PlayerRb;
    Particle_Controller particleControllerscrtipt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckPoint = transform.position;
        PlayerRb = GetComponent<Rigidbody2D>();
        particleControllerscrtipt = GetComponent<Particle_Controller>();  // ✅ Get reference to Particle_Controller
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.CompareTag("Obsticle"))
        {
            Die();
        }
    }

    void Die()
    {
        
        if (particleControllerscrtipt!= null)
        {
            particleControllerscrtipt.PlayDieParticle(transform.position);  // ✅ Trigger DieParticle
        }
        StartCoroutine(Respawn(0.5f));
    }

    public void UpdateCheckPoint(Vector2 pos)
    {
        CheckPoint = pos;
    }
    IEnumerator Respawn(float duration)
    {
        PlayerRb.simulated = false;
        PlayerRb.linearVelocity = new Vector2(0,0);
        transform.localScale = new Vector3(0,0,1);
        yield return new WaitForSeconds(duration); 
        transform.position = CheckPoint;
        transform.localScale = new Vector3(1,1,1);
        PlayerRb.simulated = true;
    }

}
