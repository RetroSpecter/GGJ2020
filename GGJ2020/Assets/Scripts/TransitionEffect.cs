using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransitionEffect : MonoBehaviour
{
    public static TransitionEffect instance;
    public GameObject blackScreen;

    private void Start()
    {
        instance = this;
        blackScreen.transform.position = Vector3.zero;
        blackScreen.transform.DOMoveX(804, 1);
    }

    Sequence s;
    public void transitionOut(int level) {
        if (s != null) return;

        s = DOTween.Sequence();
        s.Append(blackScreen.transform.DOMoveX(-804, 0));
        s.Append(blackScreen.transform.DOMoveX(0, 0.5f));
        s.AppendCallback(() => {
            Application.LoadLevel(level);
        });
    }
}
