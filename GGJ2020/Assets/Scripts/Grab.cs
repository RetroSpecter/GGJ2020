using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grab : MonoBehaviour
{
    Player player;
    Hitbox hitbox;
    private float fistInitialSize;
    public float fistChargeSize;
    public GameObject rotationPoint;

    public float launchStrength = 20;

    private GameObject grappledObject;

    public void init(Player player)
    {
        this.player = player;
        this.hitbox = GetComponent<Hitbox>();
    }

    public Sequence Action() {
        if (grappledObject == null) {
            if (hitbox.triggerCheck() == null) return null;
            grappledObject = hitbox.triggerCheck().gameObject;
            return grabAssteroid();
        } else {
            return throwAssteroid();
        }
    }

    Sequence grabAssteroid() {
        DOTween.Sequence();
        Sequence s = DOTween.Sequence();
        s.OnStart(() => {
            player.burstForce(Vector3.zero, false);
            player.gravityOn = grappledObject == null;
            grappledObject.transform.parent = rotationPoint.transform;
            grappledObject.transform.localPosition = Vector3.zero;
            // need to parent the grapple object to something
        });
        return s;
    }

    Sequence throwAssteroid()
    {
        Vector2 dir = grappledObject.transform.up;

        Debug.DrawRay(transform.position, dir * 5, Color.red, 10);
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => {
            dir *= launchStrength;
            dir.y = Mathf.Min(dir.y, 30);
            player.burstForce(-dir, false);
            player.gravityOn = true;
            player.canMove = true;
            grappledObject.transform.parent = null;
            grappledObject.GetComponent<Asteroid>().burstForce(dir * 0.01f, false);
            grappledObject = null;
        });
        s.AppendInterval(0.25f);
        return s;
    }
}
