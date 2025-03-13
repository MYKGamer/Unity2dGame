using UnityEngine;

public class CheckPoint : MonoBehaviour
{
   GameManager gameManager;

   void Awake()
   {
        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
   }
   void OnTriggerEnter2D(Collider2D collision )
   {
        if(collision.CompareTag("Player"))
        {
            gameManager.UpdateCheckPoint(transform.position);
        }
   }
}
