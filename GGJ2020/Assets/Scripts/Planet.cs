using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public int health = 3;
    public ParticleSystem explode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollision(Transform collision)
    {
        print("hey");
        if (collision.transform.GetComponent<Asteroid>() != null)
        {
            takeDamage();
        }
    }

    private void OnTrigger(Transform other)
    {
        print("asdfadfadf");
    }

    void takeDamage() {
        health--;
        if (health <= 0) {
            AudioManager.instance.Play("core_destroyed");
            GlobalEffects.instance.Screenshake(1, 10, 10);
            Instantiate(explode, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
