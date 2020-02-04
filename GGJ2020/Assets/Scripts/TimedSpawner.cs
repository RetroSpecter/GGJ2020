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

    // Start is called before the first frame update
    protected override void Start()
    {
        // do nothing   
    }

    public void StartWave(WaveManager manager) {
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
        CancelInvoke();
    }
}
