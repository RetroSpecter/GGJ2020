using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public IWave[] waves;
    public int currentWave = 0;

    // Start is called before the first frame update
    void Start() {
        waves = GetComponentsInChildren<IWave>();
        waves[currentWave].StartWave(this);
    }

    public void StartNextWave() {
        waves[currentWave].KillWave();
        currentWave++;
        if (currentWave > waves.Length) {
            print("game over");
        }
        waves[currentWave].StartWave(this);
        print("started new wave");
    }

    public void KillGame() {
        waves[currentWave].KillWave();
    }
}
