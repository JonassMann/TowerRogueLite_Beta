using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBook : MonoBehaviour
{
    public TMP_Text enemyText;

    public void SetText(string newText)
    {
        enemyText.text = newText;
    }
}
