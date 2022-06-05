using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRun : MonoBehaviour
{

    public float delayedtime = 0.2f;

   private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag != "Player") return;
        Invoke(nameof(GameScene), delayedtime);
    }

    public void GameScene()
    {
        SceneManager.LoadScene("StartingRoom");
    }
}
