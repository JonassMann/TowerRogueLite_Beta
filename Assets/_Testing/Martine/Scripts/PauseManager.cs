using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            SetTimeScale();
        }
    }
    private void SetTimeScale()
    {
        Time.timeScale = isPaused ? 0 : 1;
    }
}
