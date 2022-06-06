using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomActive : MonoBehaviour
{
    private GameObject fog;
    private GameObject room;
    private GameObject doors;
    private GameObject enemies;

    private bool roomCompleted = false;

    private int enemyNum;

    public Image mapObj;

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

        enemyNum = enemies.transform.childCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        fog.SetActive(false);
        room.SetActive(true);
        doors.SetActive(!roomCompleted);
        enemies.SetActive(true);
        GameObject.Find("MapManager").GetComponent<MapCreator>().UpdateMiniMap(transform.position);
        mapObj.color = Color.white;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        collision.GetComponent<Character>().ResetTarot();
        fog.SetActive(true);
        room.SetActive(false);
        roomCompleted = true;
        enemies.SetActive(false);
        mapObj.color = Color.green;
    }

    public void CheckCount(GameObject enemy)
    {
        enemyNum--;
        GameObject.Find("DropManager").GetComponent<ItemDrop>().DoDrop(enemy.transform.position, transform);

        //Debug.Log(enemies.transform.childCount);

        if(enemyNum <= 0)
        {
            roomCompleted = true;
            GameObject.Find("MapManager").GetComponent<MapCreator>().RoomDone();
            doors.SetActive(false);
        }

        Destroy(enemy);
    }
}
