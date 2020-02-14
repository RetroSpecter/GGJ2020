using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimedSpawner : Spawner, IWave
{
    public float time;
    public delegate void endEvent();
    public endEvent endWave;
    public float cameraZPosition;
    public AudioClip music;

     protected override void Start() { }

    public void StartWave(WaveManager manager) {
        print("start");
        if(music != null)
            MusicManager.instance.playMusic(music);
            endWave += manager.StartNextWave;
        Sequence s = DOTween.Sequence();
        s.Append(GlobalEffects.instance.MoveCamOut(cameraZPosition, 1f));
        s.AppendCallback(() => {
            InvokeRepeating("Spawn", spawnRate, spawnRate);
        });
        s.AppendInterval(time);
        s.AppendCallback(() => {
            endWave.Invoke();
            endWave = null;
            CancelInvoke();
        });
    }

    public void KillWave() {
        foreach (Asteroid asstroid in FindObjectsOfType<Asteroid>()) {
            Destroy(asstroid.gameObject);
        }
        CancelInvoke();
    }
}
