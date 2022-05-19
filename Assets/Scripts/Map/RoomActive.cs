using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActive : MonoBehaviour
{
    private GameObject fog;
    private GameObject room;
    private GameObject doors;

    private bool roomCompleted = false;

    private void Awake()
    {
        fog = transform.Find("Fog").gameObject;
        room = transform.Find("Room").gameObject;
        room = transform.Find("Doors").gameObject;
        fog.SetActive(true);
        room.SetActive(false);
        doors.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        fog.SetActive(false);
        room.SetActive(true);
        doors.SetActive(roomCompleted);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        fog.SetActive(true);
        room.SetActive(false);
        roomCompleted = true;
    }
}
