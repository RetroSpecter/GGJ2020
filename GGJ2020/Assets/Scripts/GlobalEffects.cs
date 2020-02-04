using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GlobalEffects : MonoBehaviour
{

    public static GlobalEffects instance;
    private Camera cam;

    void Awake()
    {
        instance = this;
        cam = Camera.main;

    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //Screenshake(0.5f, 0.5f, 10);
        }   
    }
    */

    public Sequence shake;
    public void Screenshake(float duration, float amp, int strength) {
        shake?.Kill();
        shake = DOTween.Sequence();
        shake.Append(cam.DOShakePosition(duration, amp, strength, 90, true));
    }

    public Sequence MoveCamOut(float pos, float time) {
        Sequence s = DOTween.Sequence();
        s.Append(cam.transform.DOMoveZ(pos, time));
        return s;
    }

    private IEnumerator currentTimeFreeze;
    public void timeFreeze(float duration) {
        if (currentTimeFreeze == null) {
            currentTimeFreeze = timeFreezeEnum(duration);
            StartCoroutine(currentTimeFreeze);
        }
    }

    private IEnumerator timeFreezeEnum(float duration) {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration/2);
        Time.timeScale = 1;
        currentTimeFreeze = null;
    }
}
