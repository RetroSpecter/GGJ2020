using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingCenter : MonoBehaviour
{

    void Update()
    {
        Vector2 dir = new Vector2(-Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
