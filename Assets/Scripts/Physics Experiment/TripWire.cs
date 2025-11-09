using UnityEngine;

public class TripWire : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public LayerMask detectionMask = Physics2D.DefaultRaycastLayers;

    private bool triggered = false;

    void Update()
    {
        Debug.DrawLine(pointA.position, pointB.position, triggered ? Color.red : Color.green);

        RaycastHit2D hit = Physics2D.Linecast(pointA.position,pointB.position, detectionMask);

        if (hit.collider != null & !triggered);
        {
            triggered = true;
            Debug.Log("Space Tripwire Triggerd");
        }
        if (hit.collider == null && triggered)
        {
            triggered = false;
        }
    }
}
