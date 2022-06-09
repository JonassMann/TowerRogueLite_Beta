using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScene : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject menu;
    public TMP_Text text;

    public void GameEnd(string endText)
    {
        text.text = endText;
        setPaused(true);
    }

    public void exit()
    {
        SceneManager.LoadScene(0);
    }

    private void setPaused(bool paused)
    {
        isPaused = paused;
        menu.SetActive(paused);
        SetTimeScale();
    }


    private void SetTimeScale()
    {
        Time.timeScale = isPaused ? 0 : 1;
    }
}