using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    PowerUpManager pum;
    RampedWaveManager waveManager;
    ShipManager shipManager;

    // common delegates used by everyone
    public delegate void GameEvent();

    // maybe split this off into its own level manager class if need be
    public int experience;
    public int experienceReq = 10;
    public int experienceReqIncrease = 10;
    private int numOfPowerups = 0; // there has to be a more graceful way to handleThis

    private void Awake() {
        instance = this;
        pum = GetComponentInChildren<PowerUpManager>();
        waveManager = GetComponentInChildren<RampedWaveManager>();
        shipManager = FindObjectOfType<ShipManager>();

        shipManager.gameOver += GameOver;
    }

    private void Start()
    {
        StartCoroutine(RunGame());
    }

    IEnumerator RunGame() {
        while (1 == 1) {
            yield return waveManager.startWave();

            while (numOfPowerups > 0) {
                numOfPowerups -= 1;
                yield return pum.startPowerupSelect();
            }
        }
    }

    // TODO: setup logic for level up UI/effect
    public void incrementExperience(int amount) {
        int temp = experience;
        experience += amount;
        if (experience > experienceReq) {
            numOfPowerups++;
            experience -= experienceReq;
            experienceReq += experienceReqIncrease;
        }
    }

    public void GameOver() {
        StopAllCoroutines();
    }
}
