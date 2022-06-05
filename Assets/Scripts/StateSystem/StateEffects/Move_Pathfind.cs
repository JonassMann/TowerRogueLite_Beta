using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move_Pathfind : StateEffect
{
    public float weight = 1;
    public GameObject target;
    public LayerMask wallMask;

    private PathGrid grid;
    private Pathfinding pathFinder;

    private int targetIndex;

    private Coroutine lastPath = null;

    private Vector3[] path;

    private Vector3 moveDir;

    private GameObject actualTarget;
    private GameObject actualUser;

    private bool active;

    private float repeatTimer;

    private void Awake()
    {
        grid = new PathGrid();
        pathFinder = new Pathfinding();

        grid.OnAwake();
        grid.unwalkableMask = wallMask;
        pathFinder.OnAwake(grid);
    }

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget)
    {
        Debug.Log(gameObject.name);
        active = false;
        CancelInvoke();
        //StopCoroutine(lastPath);
    }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget)
    {
        active = true;
        actualUser = user;
        if (this.target != null)
            actualTarget = this.target;
        else if (moveTarget != null)
            actualTarget = moveTarget;
        else return;

        //InvokeRepeating(nameof(GetPath), 0, .2f);
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        if(repeatTimer <= 0)
        {
            GetPath();
            repeatTimer = .2f;
        }
        else repeatTimer -= Time.deltaTime;

        if (moveDir != null) user.GetComponent<Character>().moveInput += (Vector2)moveDir * weight;

        return null;
    }

    private void GetPath()
    {
        if (active == false) return;
        //Debug.Log(actualTarget.transform.position);
        path = pathFinder.FindPath(actualUser.transform.position, actualTarget.transform.position);

        if (lastPath != null) StopCoroutine(lastPath);
        targetIndex = 0;
        lastPath = StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        if (path != null)
        {
            Vector3 currentWaypoint = path.Length != 0 ? path[0] : transform.position;
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                moveDir = (currentWaypoint - transform.position).normalized;
                //Debug.Log(moveDir);
                yield return null;
            }
        }
    }

    public void OnDrawGizmos()
    {
        //if (grid != null)
        //{
        //    foreach (KeyValuePair<(float, float), Node> k in grid.grid)
        //    {
        //        Gizmos.color = k.Value.walkable ? Color.white : Color.red;
        //        Gizmos.DrawCube(k.Value.worldPos, Vector3.one * .5f);
        //    }
        //}

        //if (grid != null)
        //{
        //    foreach (KeyValuePair<(float, float), Node> k in grid.grid)
        //    {
        //        Gizmos.color = k.Value.walkable ? Color.white : Color.red;
        //        Handles.Label(k.Value.worldPos, k.Value.gCost.ToString());
        //    }
        //}


        if (path != null && active)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * .1f);
                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, moveDir);
        }
    }
}
