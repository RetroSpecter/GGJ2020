using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Asteroid : MonoBehaviour
{
    private Controller2D controller;
    public float health;
    public float gravityStrength;
    public GameObject explosion;
    public float xVel;
    Vector2 velocity;
    [Space()]
    public GameObject experience;
    public int minExperience = 1, maxExperience = 3;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        velocity.x = Random.Range(0, 101) < 50 ? -xVel : xVel;
        transform.GetChild(0).transform.DORotate(Random.onUnitSphere * 360, 1);
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move((gravityStrength * Vector2.down + velocity) * Time.deltaTime);
    }

    protected virtual void OnCollision(Transform collision)
    {
        GameObject stuff = Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(stuff, 1f);
        GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject);
        
    }

    Sequence s;
    protected virtual void OnTrigger(Transform collision)
    {
        health--;
        if (health <= 0 || collision.GetComponent<Player>()) {
            GameObject stuff = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(stuff, 1f);
            GetComponent<Collider2D>().enabled = false;
            s.Kill();
            Destroy(this.gameObject);


            int numOfDrop = Random.Range(minExperience, maxExperience);
            for (int i = 0; i < numOfDrop; i++)
            {
                Instantiate(experience, transform.position, Quaternion.identity);
            }

        } else {
            burstForce(Vector2.up * 0.5f, true);
            s.Kill();
            s = DOTween.Sequence();
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
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
