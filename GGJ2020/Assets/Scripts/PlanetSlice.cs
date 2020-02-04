using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlanetSlice : MonoBehaviour
{

    private SpriteRenderer sprite;
    private PolygonCollider2D collider;
    public int active, damaged;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<PolygonCollider2D>();
    }

    private void OnCollision(Transform collision)
    {
        if (collision.transform.GetComponent<Asteroid>() != null)
        {

            DestroyLayer();
        }
    }

    public void DestroyLayer() {
        AudioManager.instance.Play("plate_hit");
        GlobalEffects.instance.Screenshake(0.5f, 0.5f, 10);
        gameObject.layer = damaged;
        sprite.DOFade(0.0f, 0.25f);
    }

    public void RepairLayer() {
        gameObject.layer = active;
        sprite.DOFade(0.5f, 0.25f);
    }
}
