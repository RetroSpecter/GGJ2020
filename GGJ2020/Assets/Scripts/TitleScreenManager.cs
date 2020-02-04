using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown) {
            TransitionEffect.instance.transitionOut((Application.loadedLevel+1) % Application.levelCount);
        }
    }
}
