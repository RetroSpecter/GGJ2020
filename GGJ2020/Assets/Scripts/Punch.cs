using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Hitbox))]
public class Punch : MonoBehaviour
{
    Sequence currentAction;
    public float punchDelay;

    Hitbox hitbox;
    void Start()
    {
        hitbox = GetComponent<Hitbox>();
    }

    public bool canAct() {
        return currentAction == null;
    }

    public void Action() {        
        Collider2D[] asteroids = hitbox.triggerAllCheck();

        currentAction = DOTween.Sequence();
        currentAction.AppendCallback(() =>
        {
            foreach (Collider2D asteroid in asteroids)
            {
                asteroid.GetComponent<Asteroid>().Hurt();
            }
        });
        currentAction.AppendInterval(punchDelay);
        currentAction.OnComplete(() => {
            currentAction = null;
        });
    }
}
