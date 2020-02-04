using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Machine : MonoBehaviour
{
    private Controller2D controller;
    public float gravity;
    private Vector3 velocity;
    bool pickedUp;
    private float maxHealth;
    public float health;
    public int active, damaged;
    public float drag = 1.2f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        maxHealth = health;
        controller = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update() {
        if (pickedUp) {

        } else {
            velocity.y -= gravity * Time.deltaTime;

            if (controller.collisions.below) {
                velocity.x /= drag;
            }

            controller.Move(velocity * Time.deltaTime);
        }
        controller.triggerCheck();
    }

    public void burstForce(Vector3 velocity, bool additive)
    {
        if (additive)
        {
            this.velocity += velocity;
        }
        else
        {
            this.velocity = velocity;
        }
    }

    public void pickUp() {
        pickedUp = true;
    }

    public void putDown() {
        pickedUp = false;
    }

    protected void OnTrigger(Transform collision)
    {
        if (collision.transform.GetComponent<Asteroid>() != null)
        {

            takeDamage();
        }
    }

    protected virtual void takeDamage()
    {
        health--;
        if (health <= 0)
        {
            AudioManager.instance.Play("machine_hit");
            foreach (SpriteRenderer sp in GetComponentsInChildren<SpriteRenderer>()) {
                sp.DOFade(0.25f, 0.25f);
            
            gameObject.layer = damaged;
}
        }
    }

    public virtual void repair()
    {
        AudioManager.instance.Play("repair");
        foreach (SpriteRenderer sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.DOFade(1, 0.25f);
        }
        gameObject.layer = active;
        health = maxHealth;
    }
}
