using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    private Slider s;
    // Start is called before the first frame update
    void Awake()
    {
        s = GetComponentInChildren<Slider>();
    }

    public void HideSlider() {
        s.gameObject.SetActive (false);
    }

    public void ShowSlider()
    {
        s.gameObject.SetActive(true);
    }

    // Update is called once per frame
    public void UpdateSlider(float value)
    {
        s.value = value;
    }
}
