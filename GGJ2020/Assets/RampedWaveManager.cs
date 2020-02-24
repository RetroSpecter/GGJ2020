using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RampedWaveManager : Spawner
{

    [SerializeField] public Enemy[] spawnRates;
    public float waveLength;
    public float breakLength;

    public string[] logs;
    public int days = 1;
    public TextMeshProUGUI captainsLog;

    // Start is called before the first frame update
    protected override void Start()
    {
        captainsLog.enabled = false;
    }

    public IEnumerator startWave()
    {
        yield return new WaitForSeconds(1);
        captainsLog.enabled = true;
        captainsLog.SetText("Captians log entry " + days + ": " + logs[Mathf.Min(days, logs.Length - 1)]);
        yield return new WaitForSeconds(breakLength);
        captainsLog.enabled = false;

        print("wave started");


        float interval = waveLength / spawnRate;
        for (int i = 0; i < interval; i++) {
            yield return new WaitForSeconds(spawnRate);
            Spawn();
        }

        days++;
        yield return new WaitForSeconds(3);
    }

    private void normalizeSpawnRate() {
        int total = 0;
        foreach (Enemy e in spawnRates) {
           // total += e.spawnChance.Evaluate(e.spawnChanceLength/days);
        }
    }

    protected override void Spawn()
    {
        //TODO: see if I can get this to be a bit more graceful
        Vector2 position = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
        GameObject targetObject = selectObject();
        Instantiate(targetObject, position, Quaternion.identity);
        float percent = multiplePercentage;
        while (percent > 10)
        {
            if (Random.Range(0f, 101f) < percent)
            {
                percent /= 2;
                position = (Vector2)transform.position + Random.insideUnitCircle.normalized * (spawnRadius + Random.Range(5, 15));
                targetObject = selectObject();
                Instantiate(targetObject, position, Quaternion.identity);
            }
            else
            {
                break;
            }
        }
    }

    GameObject selectObject() {
        return spawnRates[Random.Range(0, spawnRates.Length)].prefab;
    }
}

[System.Serializable]
public struct Enemy {
    public GameObject prefab;
    public AnimationCurve spawnChance;
    public float spawnChanceLength;
}