using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerupOptionUI : MonoBehaviour
{

    public TextMeshProUGUI powerupName, powerupButton;

    public void setPowerup(string machine, string keycode) {
        powerupName.text = machine;
        powerupButton.text = keycode;
    }
}
