using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    
    public GameObject menu;
    private Canvas menuCanvas;

    [SerializeField] private GameObject _resumeButton;
    [SerializeField] private GameObject _exitButton;


    private void Awake()
    {
        menuCanvas = menu.GetComponent<Canvas>();

        Button btn = _resumeButton.GetComponent<Button>();
        btn.onClick.AddListener(resume);

        btn = _exitButton.GetComponent<Button>();
        btn.onClick.AddListener(exit);
    }


    private void resume() {
        setPaused(false);
    }


    private void exit() {
        //  exit code
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            setPaused(!isPaused);
        }
    }


    private void setPaused(bool paused)
    {
        isPaused = paused;
        menuCanvas.enabled = paused;
        SetTimeScale();
    }


    private void SetTimeScale()
    {
        Time.timeScale = isPaused ? 0 : 1;
    }
}
