using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchCharacter : MonoBehaviour
{
    public static Vector2 lastCheckPointPos = new Vector2(0, 0);
    
    public GameObject[] playerPrefabs;
    public int characterIndex;
    public TMP_Text characterSelect;
    public GameObject canvas;

    

   private void Awake()

    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        Instantiate(playerPrefabs[characterIndex], lastCheckPointPos, Quaternion.identity);

           
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        {
            canvas.SetActive(true);
        }
    }



}
