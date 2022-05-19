using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class NewRoom : MonoBehaviour
{
    public Vector2Int roomSize;
    public bool create;

    private void Update()
    {
        if (!create) return;

        transform.position = Vector3.zero;
        gameObject.AddComponent<RoomActive>();
        gameObject.AddComponent<Room>();
        gameObject.AddComponent<BoxCollider2D>();
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(roomSize.x - .8f, roomSize.y - .8f);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

        GameObject fog = new GameObject("Fog");
        fog.transform.parent = transform;
        fog.transform.localPosition = Vector3.zero;
        fog.transform.localScale = new Vector3(roomSize.x, roomSize.y, 1);
        fog.AddComponent<SpriteRenderer>();
        fog.GetComponent<SpriteRenderer>().color = Color.black;
        fog.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/Square", typeof(Sprite));
        fog.SetActive(false);

        GameObject room = new GameObject("Room");
        room.transform.parent = transform;
        room.transform.localPosition = Vector3.zero;

        GameObject doors = new GameObject("Doors");
        doors.transform.parent = transform;
        doors.transform.localPosition = Vector3.zero;

        GameObject grid = new GameObject("Grid");
        grid.transform.parent = transform;
        grid.transform.localPosition = Vector3.zero;
        grid.AddComponent<Grid>();

        GameObject walls = new GameObject("Walls");
        walls.transform.parent = grid.transform;
        walls.transform.position = Vector3.zero;
        walls.layer = LayerMask.NameToLayer("Wall");
        walls.AddComponent<Tilemap>();
        walls.AddComponent<TilemapRenderer>();
        walls.AddComponent<TilemapCollider2D>();

        GameObject floor = new GameObject("Floor");
        floor.transform.parent = grid.transform;
        floor.transform.position = Vector3.zero;
        floor.AddComponent<Tilemap>();
        floor.AddComponent<TilemapRenderer>();

        DestroyImmediate(this);
    }
}
