using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBookWeapons : MonoBehaviour
{
    public TMP_Text weaponsText;

    public void SetText(string newText)
    {
        weaponsText.text = newText;
    }
}

