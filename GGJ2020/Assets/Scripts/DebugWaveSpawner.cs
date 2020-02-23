using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// temporary spawner class that simulates waves. 
public class DebugWaveSpawner : Spawner
{

    public float waveLength;
    public float breakLength;

    public string[] logs;
    public int days = 1;
    public TextMeshProUGUI captainsLog;

    // Start is called before the first frame update
    protected override void Start() {
        captainsLog.enabled = false;
    }

    public IEnumerator startWave() {
        yield return new WaitForSeconds(1);
        captainsLog.enabled = true;
        captainsLog.SetText("Captians log entry " + days + ": " + logs[Mathf.Min(days, logs.Length-1)]);
        yield return new WaitForSeconds(breakLength);
        captainsLog.enabled = false;

        print("wave started");

        InvokeRepeating("Spawn", spawnRate, spawnRate);
        yield return new WaitForSeconds(waveLength);
        days++;
        CancelInvoke();
        yield return new WaitForSeconds(3);        
    }
}
