using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerAsteroid : Asteroid
{
    public GameObject cralwer;
    public int minSpawn, maxSpawn;

    protected override void OnCollision(Transform collision)
    {
        spawnCrawlers();
        GetComponent<Collider2D>().enabled = false;
        base.OnCollision(collision);
    }

    protected override void OnTrigger(Transform collision)
    {
        spawnCrawlers();
        GetComponent<Collider2D>().enabled = false;
        base.OnTrigger(collision);
    }

    void spawnCrawlers() {
        int randomSpawn = Random.Range(minSpawn, maxSpawn);
        for (int i = 0; i < randomSpawn; i++) {
            Vector2 launchDir = new Vector2(0, Random.Range(5, 10));
            Crawler cralwer = Instantiate(this.cralwer, transform.position, Quaternion.identity).GetComponent<Crawler>();
            cralwer.burstForce(launchDir, true);
        }
    }
}
