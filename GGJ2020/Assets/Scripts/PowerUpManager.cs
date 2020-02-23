using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] machines;

    [Space()]
    public GameObject UI;
    public PowerupOptionUI[] powerups;
    public KeyCode[] powerupKeys;

    public float spawnRadius;
    
    public IEnumerator startPowerupSelect() {
        GameObject[] randomMachines = randomlyChoosePowerups(powerupKeys.Length);
        for (int i = 0; i < powerupKeys.Length; i++) {
            PowerupOptionUI ui = powerups[i];
            ui.setPowerup(randomMachines[i].name, powerupKeys[i].ToString());
        }

        UI.transform.DOMoveY(0, 0.5f);
        while (true) {
            for (int i = 0; i < powerupKeys.Length; i++) {
                KeyCode key = powerupKeys[i];
                if (Input.GetKeyDown(key)) {

                    spawnObject(randomMachines[i]);
                    UI.transform.DOMoveY(-403, 0.5f);
                    yield return new WaitForSeconds(0.5f);
                    yield break;
                }
            }
            yield return null;
        }
    }

    GameObject[] randomlyChoosePowerups(int num) {
        GameObject[] ret = new GameObject[num];
        for (int i = 0; i < num; i++) {
            ret[i] = machines[Random.Range(0, machines.Length)];
        }
        return ret;
    }

    void spawnObject(GameObject machine) {
        Instantiate(machine, Random.insideUnitCircle.normalized * spawnRadius, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector2.zero, spawnRadius);
    }

}
