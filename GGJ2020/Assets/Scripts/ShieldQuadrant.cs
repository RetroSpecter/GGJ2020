using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldQuadrant : MonoBehaviour
{
    public ShieldLayer[] shieldLayers;
    private int health;

    private void Start() {
        shieldLayers = GetComponentsInChildren<ShieldLayer>();
        foreach (ShieldLayer layer in shieldLayers) {
            layer.hit += takeDamage;
            layer.repair += repair;
        }
        shieldLayers[0].collider.enabled = true;
        health = shieldLayers.Length-1;
    }

    public bool needsRepair() {
        return health < shieldLayers.Length - 1;
    }

    private void takeDamage() {
        if (health < 0) {
            // do something
        } else {
            shieldLayers[shieldLayers.Length - 1 - health].DestroyLayer();
            health--;
        }
    }

    public void repair() {
        if (health < shieldLayers.Length)
        {
            health++;
            shieldLayers[shieldLayers.Length - 1 - health].RepairLayerVisual();
        }
    }
}
