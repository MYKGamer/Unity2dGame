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
    
    // Yeh flag determine karta hai ki platform ab reverse direction me move kar raha hai.
    private bool isReversing = false;
    
    // Rigidbody2D component ka reference (ensure karein ki Rigidbody2D, Kinematic mode me ho)
    private Rigidbody2D rb;
    
    // (Optional) Platform velocity calculate karne ke liye.
    private Vector3 previousPosition;
    
    // Waiting flag to stop movement for a duration
    private bool isWaiting = false;

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
        
        // Agar platform waiting state me nahi hai, to move karwayein.
        if (!isWaiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        previousPosition = transform.position;

        // Agar platform target ke bahut nazdeek pahunch jata hai aur waiting nahi hai, to wait coroutine start karo.
        if (!isWaiting && Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    // Coroutine jo platform ko 0.5 second tak rukne deti hai, phir waypoint update karti hai.
    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(0.6f);

        // Waypoint index update karne ka logic: Forward ya reverse direction ke hisaab se.
        if (!isReversing)
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                isReversing = true;
                currentWaypointIndex--;
            }
        }
        else
        {
            if (currentWaypointIndex > 0)
            {
                currentWaypointIndex--;
            }
            else
            {
                isReversing = false;
                currentWaypointIndex++;
            }
        }
        targetPosition = waypoints[currentWaypointIndex].position;
        isWaiting = false;
    }

    // Jab Player platform ke trigger zone me enter kare.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player ko platform ka child bana lo taki platform ke sath move ho (optional).
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

    // Jab Player platform se exit kare.
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
