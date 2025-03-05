using UnityEngine;

public class TouchColliderForwarder : MonoBehaviour
{
    // Specify which side this collider is for ("Left" or "Right")
    public string side;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (side == "Left")
            SendMessageUpwards("OnLeftTouchEnter", collision);
        else if (side == "Right")
            SendMessageUpwards("OnRightTouchEnter", collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (side == "Left")
            SendMessageUpwards("OnLeftTouchExit", collision);
        else if (side == "Right")
            SendMessageUpwards("OnRightTouchExit", collision);
    }
}
