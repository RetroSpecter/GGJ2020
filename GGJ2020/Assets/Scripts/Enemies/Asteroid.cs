using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Asteroid : MonoBehaviour
{
    private Controller2D controller;
    public float health;
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
        controller.Move((gravityStrength * Vector2.down + velocity) * Time.deltaTime);
    }

    private void OnCollision(Transform collision)
    {
        GameObject stuff = Instantiate(coin, transform.position, Quaternion.identity);
        Destroy(stuff, 1f);
        GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject);
        
    }

    private void OnTrigger(Transform collision)
    {
        health--;
        if (health <= 0)
        {
            GameObject stuff = Instantiate(coin, transform.position, Quaternion.identity);
            Destroy(stuff, 1f);
            GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject);
        }
        else
        {
            burstForce(Vector2.up * 0.5f, true);
            Sequence s = DOTween.Sequence();
            for (int i = 0; i < 3; i++)
            {
                s.Append(GetComponentInChildren<SpriteRenderer>().DOFade(0.25f, 0.1f));
                s.Append(GetComponentInChildren<SpriteRenderer>().DOFade(1f, 0.1f));
            }
        }
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
