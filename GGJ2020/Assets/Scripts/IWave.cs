using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWave
{
    // Start is called before the first frame update
    void StartWave(WaveManager manager);
    void KillWave();
}
