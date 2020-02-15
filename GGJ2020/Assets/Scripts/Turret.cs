using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Machine
{
    public GameObject bullet;
    public Transform shootPosition;
    public float rate;

    protected override void Start()
    {
        base.Start();
        updateRate(rate);
    }

    protected override void takeDamage()
    {
        base.takeDamage();
        if (health <= 0)
        {
            updateRate(0);
        }
    }

    public void shoot() {
        Instantiate(bullet, shootPosition.position, shootPosition.rotation);
    }

    public void updateRate(float newRate) {
        CancelInvoke();
        //AudioManager.instance.Play("turret_shoot");
        InvokeRepeating("shoot", newRate, newRate);
    }

    public override void repair()
    {
        base.repair();
        updateRate(rate);
        print("yes");

    }
}
