using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    public static DebugTools instance;

    private void Start()
    {
        //Cursor.visible = false;
        if (instance == null)
        {
            instance = this;
            Application.DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Application.LoadLevel(Application.loadedLevel);

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        //if (Input.GetKeyDown(KeyCode.UpArrow)) GoToNextLevel(1);
    }

    void GoToNextLevel(int num) {
        print("go");
        int targetLevel = Application.loadedLevel + num;
        targetLevel %= Application.levelCount;
        Application.LoadLevel(targetLevel);
    }
}
