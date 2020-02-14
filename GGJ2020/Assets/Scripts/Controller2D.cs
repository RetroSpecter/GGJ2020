using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {


    public LayerMask collisionMask;
    public LayerMask triggerMask;

    const float skinWidth = 0.015f;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigin;
    public int horizontalRayCount = 4;
    public int VerticalRayCount = 4;

    float horizontalRaySpacing;
    float VecticalRaySpacing;
    public CollisionInfo collisions;
    public Transform centerOfGravity;
    public float extraTriggerRadius = 0.1f;

    public Vector2 dirOfGravity;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (centerOfGravity != null)
        {
            dirOfGravity = centerOfGravity.transform.position - transform.position;
        } else {
            dirOfGravity =  - transform.position;
        }

        if (velocity.x != 0)
        {
            HorizontalTriggers(velocity);
            HorizontalCollisions(ref velocity);
        }

        if (velocity.y != 0)
        {
            VerticalTriggers(velocity);
            VerticalCollisions(ref velocity);
        }

        CalculateGravity();
        transform.Translate(velocity);
    }

    // for this I want constantly checking there surroundings for stuff;
    public void triggerCheck() {
        Debug.DrawRay(transform.position, Vector3.up * extraTriggerRadius, Color.green);
        Collider2D hit = Physics2D.OverlapCircle(transform.position, extraTriggerRadius, triggerMask);
        if (hit != null) {
            hit.transform.gameObject.SendMessage("OnTrigger", this.transform, SendMessageOptions.DontRequireReceiver);
            SendMessage("OnTrigger", hit.transform, SendMessageOptions.DontRequireReceiver);
        }
    }

    void CalculateGravity()
    {
        //RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.bottomLeft, -transform.up, 1, collisionMask);

        //Debug.DrawRay(transform.position, hit.normal, Color.green);
        Debug.DrawRay(transform.position, dirOfGravity, Color.blue);

        //if (hit)
        //dirOfGravity = -hit.normal;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.Normalize(dirOfGravity));

    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topRight;
            rayOrigin += (Vector2)transform.up * -directionX * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, directionX * transform.right * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;


                hit.transform.gameObject.SendMessage("OnCollision", this.transform, SendMessageOptions.DontRequireReceiver);
                SendMessage("OnCollision", hit.transform, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topRight;
            rayOrigin += (Vector2)(-directionY * transform.right) * (VecticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, transform.up * 10 * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;

                hit.transform.gameObject.SendMessage("OnCollision", this.transform, SendMessageOptions.DontRequireReceiver);
                SendMessage("OnCollision", hit.transform, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void HorizontalTriggers(Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.BottomRight;
            rayOrigin += (Vector2)transform.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.right * directionX, rayLength, triggerMask);

            //Debug.DrawRay(rayOrigin, transform.right * directionX * rayLength, Color.red);

            if (hit)
            {
                hit.transform.gameObject.SendMessage("OnTrigger", this.transform, SendMessageOptions.DontRequireReceiver);
                SendMessage("OnTrigger", hit.transform, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void VerticalTriggers(Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
            rayOrigin += (Vector2)transform.right * (VecticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up * directionY, rayLength, triggerMask);

            //Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                hit.transform.gameObject.SendMessage("OnTrigger", this.transform, SendMessageOptions.DontRequireReceiver);
                SendMessage("OnTrigger", hit.transform, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        Vector3 boundsMax = transform.TransformPoint(collider.offset + collider.size / 2);
        Vector3 boundsMin = transform.TransformPoint(collider.offset - collider.size / 2);

        raycastOrigin.bottomLeft = new Vector2(boundsMin.x, boundsMin.y);
        raycastOrigin.BottomRight = new Vector2(boundsMax.x, boundsMin.y);
        raycastOrigin.topLeft = new Vector2(boundsMin.x, boundsMax.y);
        raycastOrigin.topRight = new Vector2(boundsMax.x, boundsMax.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        VecticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, BottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below, left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

}
