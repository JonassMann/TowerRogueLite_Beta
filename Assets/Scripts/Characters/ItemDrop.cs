using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public List<GameObject> drops;


    public void DoDrop(Vector3 pos, Transform parent)
    {
        int dropNum = Random.Range(0, 4);
        if (dropNum == 3) return;

        Instantiate(drops[dropNum], pos, Quaternion.identity, parent);
    }
}
