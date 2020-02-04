using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    public float hitRadius;
    public LayerMask triggerMask;

    public Collider2D triggerCheck()
    {
        return Physics2D.OverlapCircle(transform.position, hitRadius, triggerMask);        
    }

    public Collider2D[] triggerAllCheck()
    {
        return Physics2D.OverlapCircleAll(transform.position, hitRadius, triggerMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
