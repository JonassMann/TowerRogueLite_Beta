using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapManager : MonoBehaviour
{
    public int roomCount;
    public int tRoomCount;

    public float roomSizeX;
    public float roomSizeY;

    private bool doBranch = false;

    [Range(0, 100)]
    public int branching;
    public bool drawMap;

    private Dictionary<(int, int), Border> map;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateMap();

        if (Input.GetKeyDown(KeyCode.B))
            GetComponent<MapCreator>().FillMap(map);
    }

    public void CreateMap()
    {
        Debug.Log("Creating Map");

        List<(int, int)> tempMap = new List<(int, int)>();
        Queue<(int, int)> roomList = new Queue<(int, int)>();

        tempMap.Add((0, 0));
        roomList.Enqueue(tempMap[0]);

        int maxLoops = 10000;

        while (tempMap.Count < roomCount)
        {
            maxLoops--;
            if (maxLoops <= 0)
            {
                Debug.Log("Loop error");
                break;
            }

            (int, int) activeRoom = roomList.Dequeue();

            if (Random.Range(0, 2) < 1)
            {
                List<(int, int)> nRooms = GetNeightbors(activeRoom, tempMap);
                if (nRooms.Count > 0)
                {
                    (int, int) r = Random.Range(0, 2) < 1 ? nRooms[Random.Range(0, nRooms.Count)] : GetLowest(GetNeightbors(activeRoom, tempMap), tempMap);

                    if (!doBranch || GetNeightbors(r, tempMap).Count > 2)
                    {
                        //Debug.Log("Adding Room");
                        tempMap.Add(r);
                        roomList.Enqueue(r);
                    }
                    else if (!doBranch || GetNeightbors(r, tempMap).Count > 2)
                    {
                        //Debug.Log("Adding Room");
                        tempMap.Add(r);
                        roomList.Enqueue(r);
                    }
                }
            }

            doBranch = (tempMap.Count / roomCount) < (branching / 100);

            if (GetNeightbors(activeRoom, tempMap).Count != 0)
                roomList.Enqueue(activeRoom);
        }

        map = new Dictionary<(int, int), Border>();
        foreach ((int, int) r in tempMap)
        {
            map[r] = GetBorder(r, tempMap);
        }

        doBranch = false;
        Debug.Log("Map Done");
    }

    public void AddRoom()
    {

    }

    private Border GetBorder((int, int) room, List<(int, int)> tempMap)
    {
        Border b = Border.None;

        if (tempMap.Contains((room.Item1 - 1, room.Item2))) b |= Border.Left;
        if (tempMap.Contains((room.Item1 + 1, room.Item2))) b |= Border.Right;
        if (tempMap.Contains((room.Item1, room.Item2 - 1))) b |= Border.Down;
        if (tempMap.Contains((room.Item1, room.Item2 + 1))) b |= Border.Up;

        return b;
    }

    public List<(int, int)> GetNeightbors((int, int) room, List<(int, int)> tempMap)
    {
        List<(int, int)> tempRooms = new List<(int, int)>();

        if (!tempMap.Contains((room.Item1 - 1, room.Item2))) tempRooms.Add((room.Item1 - 1, room.Item2));
        if (!tempMap.Contains((room.Item1 + 1, room.Item2))) tempRooms.Add((room.Item1 + 1, room.Item2));
        if (!tempMap.Contains((room.Item1, room.Item2 - 1))) tempRooms.Add((room.Item1, room.Item2 - 1));
        if (!tempMap.Contains((room.Item1, room.Item2 + 1))) tempRooms.Add((room.Item1, room.Item2 + 1));

        return tempRooms;
    }

    public (int, int) GetLowest(List<(int, int)> rooms, List<(int, int)> tempMap)
    {
        //Debug.Log("Finding lowest n count");
        //Debug.Log("Room count: " + rooms.Count);

        (int, int) lowRoom = (0, 0);
        int currLowest = 5;

        foreach ((int, int) i in rooms)
        {
            int nCount = GetNeightbors(i, tempMap).Count;
            if (nCount < currLowest)
            {
                lowRoom = i;
                currLowest = nCount;
            }
        }

        //Debug.Log("Lowest room count: " + lowRoom);
        return lowRoom;
    }

    //public Dictionary<(int, int), Room> GetNeightbors()
    //{
    //    Dictionary<(int, int), Room> tempRooms = new Dictionary<(int, int), Room>();



    //    return tempRooms;
    //}

    private void OnDrawGizmos()
    {
        if (map != null && drawMap)
        {
            foreach (KeyValuePair<(int, int), Border> k in map)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(new Vector3(k.Key.Item1 * roomSizeX, k.Key.Item2 * roomSizeY, 0), new Vector3(roomSizeX, roomSizeY, 0));
            }
        }


    }
}
