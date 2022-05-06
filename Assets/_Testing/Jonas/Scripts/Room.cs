using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public (int, int) worldPos;
    public Border border;

    public Room((int, int) worldPos, Border border)
    {
        this.worldPos = worldPos;
        this.border = border;
    }
}

[Flags]
public enum Border
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8,
}
