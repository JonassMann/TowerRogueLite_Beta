using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActive : MonoBehaviour
{
    private GameObject fog;
    private GameObject room;
    private GameObject doors;
    private GameObject enemies;

    private bool roomCompleted = false;

    private void Awake()
    {
        fog = transform.Find("Fog").gameObject;
        room = transform.Find("Room").gameObject;
        doors = transform.Find("Doors").gameObject;
        enemies = transform.Find("Enemies").gameObject;
        fog.SetActive(true);
        room.SetActive(false);
        doors.SetActive(false);
        enemies.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        fog.SetActive(false);
        room.SetActive(true);
        doors.SetActive(!roomCompleted);
        enemies.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        collision.GetComponent<Character>().ResetTarot();
        fog.SetActive(true);
        room.SetActive(false);
        roomCompleted = true;
        enemies.SetActive(false);
    }

    public void CheckCount(GameObject enemy)
    {
        GameObject.Find("DropManager").GetComponent<ItemDrop>().DoDrop(enemy.transform.position);

        Debug.Log(enemies.transform.childCount);

        if(enemies.transform.childCount <= 1)
        {
            roomCompleted = true;
            doors.SetActive(false);
        }

        Destroy(enemy);
    }
}
