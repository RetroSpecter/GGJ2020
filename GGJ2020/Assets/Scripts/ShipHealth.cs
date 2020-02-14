using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipHealth : MonoBehaviour
{
    public int health = 3;
    public delegate void GameEvent();
    public GameEvent gameOver;

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GlobalEffects.instance.Screenshake(0.5f, 0.5f, 10);
            death();
        }
    }
    */

    private void OnCollision(Transform collision)
    {
        if (collision.transform.GetComponent<Asteroid>() != null)
        {
            takeDamage();
        }
    }

    void takeDamage() {
        health--;
        if (health <= 0) {
            death();
        }
    }

    public void death() {
        GetComponent<Collider2D>().enabled = false;
        gameOver?.Invoke();
    }
}
