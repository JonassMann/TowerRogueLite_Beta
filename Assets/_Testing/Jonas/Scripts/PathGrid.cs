using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid
{
    public LayerMask unwalkableMask;
    public float nodeRadius;

    public Dictionary<(float, float), Node> grid;

    public void OnAwake()
    {
        grid = new Dictionary<(float, float), Node>();
        nodeRadius = .4f;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                float checkX = node.worldPos.x + x;
                float checkY = node.worldPos.y + y;

                if (!grid.ContainsKey((checkX, checkY)))
                    CreateNode(checkX, checkY);

                neighbors.Add(grid[(checkX, checkY)]);
            }

        return neighbors;
    }

    public Node WorldPointToNode(Vector3 worldPos)
    {
        //float x = Mathf.Round(worldPos.x * 2f) * .5f;
        //float y = Mathf.Round(worldPos.y * 2f) * .5f;

        int xTemp = (int)worldPos.x;
        int yTemp = (int)worldPos.y;

        float x = xTemp + (worldPos.x > 0 ? .5f : -.5f);
        float y = yTemp + (worldPos.y > 0 ? .5f : -.5f);

        if (!grid.ContainsKey((x, y)))
            CreateNode(x, y);
        return grid[(x, y)];
    }

    private void CreateNode(float x, float y)
    {
        Vector3 worldPoint = new Vector3(x, y, 0);
        bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);

        grid[(x, y)] = new Node(walkable, worldPoint);
    }
}
