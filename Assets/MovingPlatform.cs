using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Waypoints array jismein aap multiple points (A, B, C, …) assign kar sakte hain.
    public Transform[] waypoints;
    
    // Platform ki movement speed.
    public float speed = 2f;
    
    // Current target position jiske taraf platform move karega.
    private Vector3 targetPosition;
    
    // Waypoints array me current index.
    private int currentWaypointIndex = 0;
    
    // Platform ke Rigidbody2D component ka reference (ensure karein ki Rigidbody2D, Kinematic mode me ho)
    private Rigidbody2D rb;

    // Platform velocity calculate karne ke liye (agar future me zarurat ho)
    private Vector3 previousPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (waypoints.Length > 0)
        {
            // Pehla waypoint as initial target set karo.
            targetPosition = waypoints[currentWaypointIndex].position;
        }
        previousPosition = transform.position;
    }

    void Update()
    {
        if (waypoints.Length == 0)
            return; // Agar koi waypoints assign nahi kiye gaye, to kuch na karo.
        
        // Platform ko current position se targetPosition tak moveTowards se move karwayein.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // (Optional) Agar aap platform ki velocity calculate karna chahte hain:
        // Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;

        // Agar platform target position ke itna pass aa gaya ho, to next waypoint par switch karo.
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            targetPosition = waypoints[currentWaypointIndex].position;
        }
    }

    // Jab Player platform ke trigger zone me enter kare
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player ko platform ka child bana lo taki platform ke sath move ho (optional)
            collision.transform.parent = transform;
            
            // Player_Movement script ko access karke platform ke related variables set karo.
            Player_Movement playerMovement = collision.GetComponent<Player_Movement>();
            if (playerMovement != null)
            {
                playerMovement.IsOnPlatform = true;
                playerMovement.PlatformRB = rb;
            }
        }
    }

    // Jab Player platform se exit kare
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
            
            Player_Movement playerMovement = collision.GetComponent<Player_Movement>();
            if (playerMovement != null)
            {
                playerMovement.IsOnPlatform = false;
                playerMovement.PlatformRB = null;
            }
        }
    }
}
