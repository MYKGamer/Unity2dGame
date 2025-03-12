using UnityEngine;
using System.Collections;  // Missing namespace for IEnumerator

public class GameManager : MonoBehaviour
{
    Vector2 SpawnPos;
    Rigidbody2D PlayerRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPos = transform.position;
        PlayerRb = GetComponent<Rigidbody2D>();
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
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        PlayerRb.simulated = false;
        PlayerRb.linearVelocity = new Vector2(0,0);
        transform.localScale = new Vector3(0,0,1);
        yield return new WaitForSeconds(duration); 
        transform.position = SpawnPos;
        transform.localScale = new Vector3(1,1,1);
        PlayerRb.simulated = true;
    }

}
