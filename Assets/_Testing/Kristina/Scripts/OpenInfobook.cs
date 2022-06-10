using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInfobook : MonoBehaviour
{
    public GameObject canvasBook;
    public GameObject canvasButtons;
    public GameObject canvasBookWeapons;
    public GameObject canvasBookTarot;
       

    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            {
            canvasBook.SetActive(true);
            canvasButtons.SetActive(true);
            canvasBookWeapons.SetActive(true);
            canvasBookTarot.SetActive(true);
            }

        }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        {
            canvasBook.SetActive(false);
            canvasButtons.SetActive(false);
            canvasBookWeapons.SetActive(false);
            canvasBookTarot.SetActive(false);
        }
    }
}

