using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipManager : MonoBehaviour
{
    public static ShipManager instance;
    public int health = 3;
    public delegate void GameEvent();
    public GameEvent gameOver;

    //TODO: eventually make this flexible enough to have more than 4 sections
    public ShieldQuadrant[] quadrants;
   
    private void Awake()
    {
        instance = this;
    }

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

    //TODO: switch to using this for collisions versus what I am currently doing
    public ShieldQuadrant getQuadrant(float angle) {
        return quadrants[(int)(angle / (360 / quadrants.Length))];
    }
}
