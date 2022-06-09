using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInfobook : MonoBehaviour
{
    public GameObject canvas;
       

    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            {
                canvas.SetActive(true);
            }

        }
    }

