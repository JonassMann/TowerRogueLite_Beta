using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPush : MonoBehaviour
{
    private float startTime;
    private bool doMove = false;

    private Vector3 startPos;
    private Vector3 endPos;

    private void OnEnable()
    {
        startPos = transform.position;
        endPos = transform.position + (transform.up * 1);

        startTime = Time.time;
        doMove = true;
    }

    private void Update()
    {
        if (!doMove) return;

        transform.position = Vector3.Lerp(startPos, endPos, ((Time.time - startTime) * 2f) / 1);
    }
}
