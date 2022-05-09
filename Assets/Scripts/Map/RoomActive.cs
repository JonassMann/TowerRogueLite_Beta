using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActive : MonoBehaviour
{
    private GameObject fog;
    private GameObject room;

    private void Awake()
    {
        fog = transform.Find("Fog").gameObject;
        room = transform.Find("Room").gameObject;
        fog.SetActive(true);
        room.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        fog.SetActive(false);
        room.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        fog.SetActive(true);
        room.SetActive(false);
    }
}
