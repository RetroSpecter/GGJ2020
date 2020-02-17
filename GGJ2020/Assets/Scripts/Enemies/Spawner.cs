using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] spawnedObject;
    public float spawnRadius;
    public float spawnRate = 2;
    public float multiplePercentage = 25f;
    [Header("debug options")]
    public Color debugColor;

    protected virtual void Start()
    {
        InvokeRepeating("Spawn", spawnRate, spawnRate);
    }

    void Spawn() {
        Vector2 position = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
        GameObject targetObject = spawnedObject[Random.Range(0, spawnedObject.Length)];
        Instantiate(targetObject, position, Quaternion.identity);
        float percent = multiplePercentage;
        while (percent > 10)
        {
            if (Random.Range(0f, 101f) < percent) {
                percent /= 2;
                position = (Vector2)transform.position + Random.insideUnitCircle.normalized * (spawnRadius + Random.Range(5, 15));
                targetObject = spawnedObject[Random.Range(0, spawnedObject.Length)];
                Instantiate(targetObject, position, Quaternion.identity);
            }
            else
            {
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
