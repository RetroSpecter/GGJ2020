using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUpManager : MonoBehaviour
{
    public GameObject turret;
    public GameObject machine;
    public GameObject UI;

    public Transform spawnPoint;
    public bool active;

    public void Activate() {
        UI.transform.DOMoveY(0, 0.5f);
        active = true;
    }

    public void Update()
    {
        if (active) {
            /*
            if (Input.GetButtonDown("Fire2"))
            {
                spawnObject(turret);
                Deactivate();
            } else if (Input.GetButtonDown("Fire3"))
            {
                spawnObject(machine);
                Deactivate();
            }
            */
        }
    }

    public void Deactivate() {
        UI.transform.DOMoveY(-403, 0.5f);
        active = false;
    }

    void spawnObject(GameObject machine) {
        Instantiate(machine, spawnPoint.position, Quaternion.identity);
    }
    
}
