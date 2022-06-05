using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapCreator : MonoBehaviour
{
    public float roomSizeX;
    public float roomSizeY;

    private Dictionary<Border, List<GameObject>> rooms;
    public GameObject startingRoom;

    private Minimap miniMap;

    private void Awake()
    {
        rooms = new Dictionary<Border, List<GameObject>>();

        Object[] roomObjects = Resources.LoadAll("Rooms", typeof(GameObject));

        //miniMap = GameObject.Find("Minimap").GetComponent<Minimap>();

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

        foreach (KeyValuePair<Border, List<GameObject>> k in rooms)
        {
            Debug.Log(k.Key);
        }
    }

    public void FillMap(Dictionary<(int, int), Border> map)
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        foreach (KeyValuePair<(int, int), Border> k in map)
            Instantiate(k.Key == (0, 0) ? startingRoom : GetRoom(k.Value), new Vector3(k.Key.Item1 * roomSizeX, k.Key.Item2 * roomSizeY, 0), Quaternion.identity, transform);
    }

    private GameObject GetRoom(Border b)
    {
        //if ((int)b == -1 || b == (Border)(-1)) return rooms[(Border)(-1)][Random.Range(0, rooms[b].Count)];
        return rooms[b][Random.Range(0, rooms[b].Count)];
    }
}
