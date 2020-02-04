using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioSource channel1;
    public AudioSource channel2;
    public bool playChannel1;

    void Awake()
    {
        instance = this;
    }

    public void playMusic(AudioClip music) {
        channel1.clip = music;
        channel1.Play();
    }

    void setVolume(float volume, AudioSource channel) {
        channel.volume = volume;
    }
}
