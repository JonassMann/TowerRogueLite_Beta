using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwitchCharacter : MonoBehaviour
{
    public static Vector2 lastCheckPointPos = new Vector2(0, 0);

    private GameObject currentPlayerObject;

    public GameObject[] playerPrefabs;

    public GameObject canvasCharacter;

    private void Awake()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacter", 0);

        currentPlayerObject = Instantiate(playerPrefabs[index], lastCheckPointPos, Quaternion.identity);
    }

    public void SelectCharacter(int index)
    {
        PlayerPrefs.SetInt("SelectedCharacter", index);
        Destroy(currentPlayerObject);
        currentPlayerObject = Instantiate(playerPrefabs[index], lastCheckPointPos, Quaternion.identity);

        canvasCharacter.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        {
            canvasCharacter.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        canvasCharacter.SetActive(false);
    }
}


