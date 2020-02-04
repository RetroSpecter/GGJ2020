using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BreakWave : MonoBehaviour, IWave
{
    public float leadtime;
    public float audioTime;
    public float endTime;
    public delegate void endEvent();
    public endEvent endWave;
    public AudioClip audio;
    private AudioSource source;
    public float cameraZPosition;
    public PowerUpManager pum;
    public bool startWave = true;
    public AudioClip music;
    public bool end = false;

    void Start() {
        source = GetComponent<AudioSource>();
    }

    public void StartWave(WaveManager manager)
    {

        if (end) {
            TransitionEffect.instance.transitionOut((Application.loadedLevel + 1) % Application.levelCount);
        }

        MusicManager.instance.playMusic(music);
        if (!startWave) {
            foreach (Asteroid asstroid in FindObjectsOfType<Asteroid>())
            {
                Destroy(asstroid.gameObject);
            }
        }

        pum?.Activate();
        endWave += manager.StartNextWave;
        Sequence s = DOTween.Sequence();
        s.AppendInterval(leadtime);
        s.Append(GlobalEffects.instance.MoveCamOut(cameraZPosition, 1f));
        s.AppendCallback(() => {
            PlayCutscene();
        });
        s.AppendInterval(audioTime);
        s.AppendInterval(endTime);
        s.AppendCallback(() => {
            pum?.Deactivate();
            endWave.Invoke();
            endWave = null;
            CancelInvoke();
        });
    }

    public void PlayCutscene() {
        source.clip = audio;
        source.Play();
    }

    public void KillWave() {
        CancelInvoke();
    }
}
