using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    Rigidbody2D rb;
    public int speed;
    float speedMultiplyier;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float targetSpeed = speed* speedMultiplyier;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
    }
}
