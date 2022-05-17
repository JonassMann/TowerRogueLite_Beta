using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    PathGrid grid;

    public void OnAwake(PathGrid grid)
    {
        this.grid = grid;
    }

    public Vector3[] FindPath(Vector3 start, Vector3 target)
    {
        Node startNode = grid.WorldPointToNode(start);
        Node targetNode = grid.WorldPointToNode(target);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            int testCounter = 0;

            while (openSet.Count > 0)
            {
                if (testCounter > 1000)
                {
                    Debug.Log("Pathfinder error");
                    break;
                }

                List<Node> testList = openSet.GetAll();
                Node testNode = testList[0];

                for (int i = 0; i < testList.Count; i++)
                {
                    if (testList[i].fCost < testNode.fCost || testList[i].fCost == testNode.fCost && testList[i].hCost < testNode.hCost)
                    {
                        testNode = testList[i];
                    }
                }


                Node currentNode = openSet.RemoveFirst();
                // Debug.Log(testNode.fCost + " _ " + currentNode.fCost);
                testCounter++;
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode, target);
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;
                    if (Physics2D.Linecast(currentNode.worldPos, neighbor.worldPos, grid.unwalkableMask)) continue;

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }

        return null;
    }

    private Vector3[] RetracePath(Node startNode, Node endNode, Vector3 targetPos)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        int testCounter = 0;

        while (currentNode != startNode)
        {
            testCounter++;
            if (testCounter > 1000)
            {
                Debug.Log("Retrace error");
                break;
            }

            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);

        Vector3[] waypoints = SimplifyPath(path, targetPos);
        return waypoints;

        //List<Vector3> tempPath = new List<Vector3>();
        //for (int i = 0; i < path.Count; i++)
        //{
        //    tempPath.Add(path[i].worldPos);
        //}
        //tempPath.Reverse();
        //return tempPath.ToArray();
    }

    Vector3[] SimplifyPath(List<Node> path, Vector3 targetPos)
    {
        List<Vector3> waypoints = new List<Vector3>();

        int tempNodePos = path.Count - 1;
        int tempCounter = 0;

        for (int i = 0; tempNodePos != 0; i++)
        {
            //if (i == 0 && !Physics2D.Linecast(path[tempNodePos].worldPos, targetPos, grid.unwalkableMask))
            if (i == 0 && !Physics2D.CircleCast(path[tempNodePos].worldPos, .4f, targetPos - path[tempNodePos].worldPos, Vector3.Distance(targetPos, path[tempNodePos].worldPos), grid.unwalkableMask))
            {
                waypoints.Add(targetPos);
                break;
            }

            //if (!Physics2D.Linecast(path[tempNodePos].worldPos, path[i].worldPos, grid.unwalkableMask))
            if (!Physics2D.CircleCast(path[tempNodePos].worldPos, .4f, path[i].worldPos - path[tempNodePos].worldPos, Vector3.Distance(path[i].worldPos, path[tempNodePos].worldPos), grid.unwalkableMask))
            {
                waypoints.Add(path[i].worldPos);
                tempNodePos = i;
                i = -1;
                tempCounter++;
                if (tempCounter >= 20)
                {
                    Debug.Log("Loop machine broke");
                    break;
                }
            }
        }

        return waypoints.ToArray();
    }

    //Vector3[] SimplifyPath(List<Node> path, Vector3 targetPos)
    //{
    //    List<Vector3> waypoints = new List<Vector3>();

    //    int tempNodePos = path.Count - 1;
    //    int tempCounter = 0;

    //    for (int i = 0; tempNodePos != 0; i++)
    //    {
    //        if (i == 0 && !Physics2D.Linecast(path[tempNodePos].worldPos, targetPos, grid.unwalkableMask))
    //        {
    //            waypoints.Add(targetPos);
    //            break;
    //        }

    //        if (!Physics2D.Linecast(path[tempNodePos].worldPos, path[i].worldPos, grid.unwalkableMask))
    //        {
    //            waypoints.Add(path[i].worldPos);
    //            tempNodePos = i;
    //            i = -1;
    //            tempCounter++;
    //            if (tempCounter >= 7)
    //            {
    //                Debug.Log("Loop machine broke");
    //                break;
    //            }
    //        }
    //    }

    //    return waypoints.ToArray();
    //}

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.RoundToInt(Mathf.Abs(nodeA.worldPos.x - nodeB.worldPos.x));
        int dstY = Mathf.RoundToInt(Mathf.Abs(nodeA.worldPos.y - nodeB.worldPos.y));

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
