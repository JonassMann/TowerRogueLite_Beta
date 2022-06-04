using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRun : MonoBehaviour
{

    public float delayedtime = 0.2f;

   private void OnTriggerEnter2D (Collider2D other)
    {
        Invoke("StartingRoom", delayedtime);
    }

    public void GameScene()
    {
        SceneManager.LoadScene("StartingRoom");
    }
}
