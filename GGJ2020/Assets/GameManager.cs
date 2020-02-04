using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    public GameObject planet;
    Sequence s;
    void Update() {
        if (planet == null && s == null) {
            s = DOTween.Sequence();
            s.AppendInterval(3);
            s.AppendCallback(() =>
            {
                TransitionEffect.instance.transitionOut((Application.loadedLevel + 2) % Application.levelCount);
            });
        }
    }
}
