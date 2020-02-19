using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Machine
{
    public float repairTime;
    IEnumerator curRepairEnum;
    private Healthbar healtbar;
    public delegate void repairDelegate();

    protected override void Start()
    {
        healtbar = GetComponentInChildren<Healthbar>();
        healtbar?.HideSlider();
        base.Start();
    }

    protected override void movement() {
        if (controller.collisions.below) {
            velocity.y = 0;
            velocity.x /= drag;
        }

        if (controller.collisions.right || controller.collisions.left)
        {
            burstForce(Vector2.up * 2, false);
        }

        int angle = Mathf.RoundToInt(get360Angle((Vector2)transform.position, Vector2.right));
        angle = (angle / 5) * 5;

        //TODO: don't hard code angles when I swtich to a more than 4 quad system
        if (canRepair() && angle % 45 == 0 && angle % 90 != 0 && QuadrantNeedsHealing(angle) != null) {
            controller.Move((velocity) * Time.deltaTime);

            repairDelegate repairShip = (() => {
                QuadrantNeedsHealing(angle).GetComponent<ShieldQuadrant>().repair();
                burstForce(Vector2.up * 3, false);
            });

            curRepairEnum = RepairEnum(repairTime, QuadrantNeedsHealing(angle), repairShip);
            StartCoroutine(curRepairEnum);
        } else if (curRepairEnum == null) {
            controller.Move((velocity + Vector3.right * horizVelocity) * Time.deltaTime);
            StopCoroutine(curRepairEnum);
            curRepairEnum = null;
            healtbar?.HideSlider();
        }
    }

    IEnumerator RepairEnum(float time, ShieldQuadrant quadrent, repairDelegate repairAction) {
        healtbar?.ShowSlider();
        float t = 0;
        float soundtime = 0;
        while (t < repairTime && quadrent.needsRepair())
        {
            healtbar?.UpdateSlider(t / repairTime);

            if (t > soundtime)
            {
                AudioManager.instance.Play("repair");
                soundtime += repairTime / 3;
            }

            t += Time.deltaTime;
            yield return null;
        }
        healtbar?.HideSlider();

        curRepairEnum = null;

        if (t >= repairTime && quadrent.needsRepair())
        {
            repairAction.Invoke();
        }

    }

    private bool canRepair() {
        return health > 0 && curRepairEnum == null && !pickedUp;
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

    protected override void takeDamage()
    {
        base.takeDamage();
        StopCoroutine(curRepairEnum);
        curRepairEnum = null;
        healtbar?.HideSlider();
    }

}
