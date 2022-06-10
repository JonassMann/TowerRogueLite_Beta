using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InfoBookTarot : MonoBehaviour
{
    
        public TMP_Text tarotText;

        public void SetText(string newText)
        {
            tarotText.text = newText;
        }
    }


