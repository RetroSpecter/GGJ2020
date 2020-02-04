using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Asteroid : MonoBehaviour
{
    private Controller2D controller;
    public float gravityStrength;
    public GameObject coin;
    public float xVel;
    Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        velocity.x = Random.Range(-1, 1) * Random.Range(0, xVel);
        transform.GetChild(0).transform.DORotate(Random.onUnitSphere * 360, 1);
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(gravityStrength * Vector2.down + velocity);
    }

    private void OnCollision(Transform collision)
    {
        Instantiate(coin, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnTrigger(Transform collision)
    {

            Instantiate(coin, transform.position, Quaternion.identity);
        
        
        AudioManager.instance.Play("asteroid_destoryed");
        GlobalEffects.instance.Screenshake(0.25f, 0.5f, 10);
        Destroy(this.gameObject);
    }

    public void burstForce(Vector2 velocity, bool additive)
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

    public void Hurt() {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
