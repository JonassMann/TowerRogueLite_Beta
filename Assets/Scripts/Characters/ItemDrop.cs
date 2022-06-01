using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public void DoDrop(GameObject drop)
    {
        // Spawn drops

        Destroy(drop);
    }
}
