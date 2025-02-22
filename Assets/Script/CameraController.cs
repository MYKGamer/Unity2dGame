using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Player ka transform jise follow karna hai.
    public Transform player;
    
    // Camera follow karne ke liye smoothness factor.
    public float smoothSpeed = 0.125f;
    
    // Player ke relative offset ko set karne ke liye.
    public Vector3 offset;

    // X and Y axis ke limitations ke liye boundaries.
    public float minX, maxX;
    public float minY, maxY;

    // LateUpdate ensure karta hai ki camera update player ke movement ke baad ho.
    void LateUpdate()
    {
        if (player != null)
        {
            // Player ki position me offset add karke desired position calculate karein.
            Vector3 desiredPosition = player.position + offset;

            // X aur Y axis ke liye clamping (limitations) apply karein.
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
            
            // Smoothly camera position ko desired position ki taraf lerp karte hain.
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
