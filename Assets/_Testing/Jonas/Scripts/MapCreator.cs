using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MapCreator : MonoBehaviour
{
    public float roomSizeX;
    public float roomSizeY;

    private Dictionary<Border, List<GameObject>> rooms;

    private int roomsToComplete;
    public GameObject startingRoom;
    public GameObject bossRoom;

    private bool bossSpawned = false;

    public GameObject mapPrefab;
    private GameObject miniMap;

    private void Awake()
    {
        rooms = new Dictionary<Border, List<GameObject>>();

        Object[] roomObjects = Resources.LoadAll("Rooms", typeof(GameObject));

        miniMap = GameObject.Find("MapObjects");

        foreach (GameObject g in roomObjects)
        {
            Border b = g.GetComponent<Room>().border;

            if ((int)b == -1)
            {
                if (!rooms.ContainsKey((Border)15)) rooms[(Border)15] = new List<GameObject>();
                rooms[(Border)15].Add(g);
            }
            else
            {
                if (!rooms.ContainsKey(b)) rooms[b] = new List<GameObject>();
                rooms[b].Add(g);
            }
        }

        //foreach (KeyValuePair<Border, List<GameObject>> k in rooms)
        //{
        //    Debug.Log(k.Key);
        //}
    }

    public void RoomDone()
    {
        roomsToComplete--;
        if (roomsToComplete > 0 || bossSpawned) return;
        bossSpawned = true;

        Destroy(transform.GetChild(0).gameObject);
        GameObject bossRoomObj = Instantiate(bossRoom, new Vector3(0, 0, 0), Quaternion.identity, transform);
        bossRoomObj.GetComponent<RoomActive>().mapObj = miniMap.transform.GetChild(0).GetComponent<Image>();
        bossRoomObj.GetComponent<RoomActive>().mapObj.color = Color.red;
    }

    public void UpdateMiniMap(Vector3 pos)
    {
        miniMap.GetComponent<RectTransform>().localPosition = -pos;
    }

    public void FillMap(Dictionary<(int, int), Border> map)
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        roomsToComplete = map.Count;

        foreach (KeyValuePair<(int, int), Border> k in map)
        {
            GameObject mapObj = Instantiate(mapPrefab, miniMap.transform);
            mapObj.GetComponent<RectTransform>().localPosition = new Vector3(k.Key.Item1 * 21, k.Key.Item2 * 21);
            GameObject room = Instantiate(k.Key == (0, 0) ? startingRoom : GetRoom(k.Value), new Vector3(k.Key.Item1 * roomSizeX, k.Key.Item2 * roomSizeY, 0), Quaternion.identity, transform);
            room.GetComponent<RoomActive>().mapObj = mapObj.GetComponent<Image>();
        }
    }

    private GameObject GetRoom(Border b)
    {
        //if ((int)b == -1 || b == (Border)(-1)) return rooms[(Border)(-1)][Random.Range(0, rooms[b].Count)];
        return rooms[b][Random.Range(0, rooms[b].Count)];
    }
}
