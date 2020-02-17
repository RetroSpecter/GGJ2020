using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Machine
{
    public float healTime;

    // Update is called once per frame
    protected override void movement()
    {
        velocity.y -= gravity * Time.deltaTime;
        if (controller.collisions.below)
        {
            velocity.y = 0;
            velocity.x /= drag;
        }

        int angle = Mathf.RoundToInt(get360Angle((Vector2)transform.position, Vector2.right));
        angle = (angle / 5) * 5;

        if (angle % 45 == 0 && angle % 90 != 0 && QuadrantNeedsHealing(angle) != null) {
            controller.Move((velocity) * Time.deltaTime);
        } else {
            controller.Move((velocity + Vector3.right * horizVelocity) * Time.deltaTime);
        }
        controller.Move((velocity + Vector3.right * horizVelocity) * Time.deltaTime);
    }

    private float get360Angle(Vector2 from, Vector2 to) {
        float ret = Vector2.SignedAngle(from, to);
        if (ret < 0) {
            ret += 360;
        }
        return ret;
    }

    ShieldQuadrant QuadrantNeedsHealing(float angle) {
        ShieldQuadrant quadrant = ShipManager.instance.getQuadrant(angle);
        if (quadrant.needsRepair())
            return quadrant;
        else
            return null;
    }
}
