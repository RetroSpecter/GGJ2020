using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Controller2D controller;
    public float speed;
    public float radius = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, radius / (speed * Time.deltaTime));
        controller = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update() {
        controller.Move(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTrigger(Transform other)
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject);
    }
}
