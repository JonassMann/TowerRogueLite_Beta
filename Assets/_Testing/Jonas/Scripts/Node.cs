using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public Node parent;

    public int gCost;
    public int hCost;

    private int heapIndex;

    public Node(bool walkable, Vector3 worldPos)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }


    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
