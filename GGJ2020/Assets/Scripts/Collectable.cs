using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectable : MonoBehaviour
{

    private Controller2D controller;
    private SpriteRenderer sprite;

    public float gravity;
    public float bounciness;
    public float drag;
    public float lifetime = 10;
    public int experienceAmount = 1;
    public float minVelocity, maxVelocity;
    private Vector3 velocity;

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponent<Controller2D>();
        velocity.x = Random.Range(minVelocity, maxVelocity);
        velocity.x *= Random.Range(0.0f, 1.0f) > 0.5f ? 1: -1;
        velocity.y = Random.Range(5, 10);
        sprite.transform.DORotate(new Vector3(0,0,-360*4), lifetime, RotateMode.FastBeyond360);
    }

    void Update() {
        if (controller.collisions.below) {
            bounce();
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTrigger(Transform collision)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GameManager.instance?.incrementExperience(experienceAmount);
        Destroy(this.gameObject);
    }

    void bounce() {
        velocity.x /= drag;
        velocity.y = -velocity.y * bounciness;
    }
}
