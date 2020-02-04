using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    private Controller2D controller;
    public float gravity;
    public float initialBounceStrength;
    public float lifetime = 10;
    public float minVelocity, maxVelocity;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        velocity.x = Random.Range(minVelocity, maxVelocity);
        velocity.x = Random.Range(0.0f, 1.0f) > 0.5f ? 1: -1;
    }

    // Update is called once per frame
    void Update() {
        if (controller.collisions.below) {
            bounce();
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTrigger(Transform collision)
    {
        Destroy(this.gameObject);
    }

    void bounce() {
        velocity.x /= 2;
        velocity.y = -velocity.y/2;
    }
}
