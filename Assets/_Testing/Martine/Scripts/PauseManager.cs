using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    
    public GameObject menu;

    //[SerializeField] private GameObject _resumeButton;
    //[SerializeField] private GameObject _exitButton;

   

    //private void Awake()
    //{
    //    Button btn = _resumeButton.GetComponent<Button>();
    //    btn.onClick.AddListener(resume);

    //    btn = _exitButton.GetComponent<Button>();
    //    btn.onClick.AddListener(exit);
    //}


    public void resume() 
    {
        setPaused(false);
    }


    public void exit() {
        SceneManager.LoadScene(0);
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
        menu.SetActive(paused);
        SetTimeScale();
    }


    private void SetTimeScale()
    {
        Time.timeScale = isPaused ? 0 : 1;
    }
}
