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

    public GameObject canvas;

    public GameObject button; 
    private Button[] buttons;


    private Button[] GetButtonList()
    {
        return button.GetComponentsInChildren<Button>();
    }

    private void Awake()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacter", 0);

        currentPlayerObject = Instantiate(playerPrefabs[index], lastCheckPointPos, Quaternion.identity);
        
        buttons = GetButtonList();
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(delegate{SelectCharacter(b.transform.GetSiblingIndex()); });
        }

        void SelectCharacter(int index)
        {
            Destroy(currentPlayerObject);
            currentPlayerObject = Instantiate(playerPrefabs[index], lastCheckPointPos, Quaternion.identity);
            
            //// fikser ikke missing reference problemet
            //FindObjectOfType<CameraController>().UpdateTarget();
            
            canvas.GetComponent<Canvas>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        {
            canvas.GetComponent<Canvas>().enabled = true;
        }
    }
}
