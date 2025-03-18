using UnityEngine;

public class CheckPoint : MonoBehaviour
{
   GameManager gameManager;
   public Transform RespawnPoint;
   SpriteRenderer spriteRenderer;
   public Sprite passive, active;

   void Awake()
   {
        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
   }
   void OnTriggerEnter2D(Collider2D collision )
   {
        if(collision.CompareTag("Player"))
        {
            gameManager.UpdateCheckPoint(RespawnPoint.position);
            spriteRenderer.sprite = active;
        }
   }
}
