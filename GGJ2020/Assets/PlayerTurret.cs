using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{

    public GameObject bullet;
    public Transform shootPosition;
    public float rate;


    public void startShot(string key) {
        StartCoroutine(_shootRate(key));
    }

    IEnumerator _shootRate(string key) {
        shoot();
        float t = 0;
        while (Input.GetButton(key)) {           
            t += Time.deltaTime;
            if (t > rate) {
                shoot();
                t = 0;
            }
            yield return null;
        }
    }

    void shoot()
    {
        Instantiate(bullet, shootPosition.position, shootPosition.rotation);
    }
}
