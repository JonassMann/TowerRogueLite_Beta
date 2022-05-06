using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Wander : StateEffect
{
    public float range;
    public bool square;
    public bool grid;
    public bool free;
    public bool returnOnChange;
    public LayerMask wallMask;
    public float walkTime = 10;

    private Vector3 startPoint;
    private GameObject targetObject;
    private float timer;

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget)
    {
        GetComponent<State>().ResetTarget();
    }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget)
    {
        timer = walkTime;

        if(startPoint == Vector3.zero || !returnOnChange)
            startPoint = user.transform.position;

        if(targetObject == null)
            targetObject = new GameObject();
        GetTarget(user);
        GetComponent<State>().SetTarget(targetObject);
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        timer -= Time.deltaTime;
        if (timer <= 0 || Vector3.Distance(user.transform.position, moveTarget.transform.position) < .1f)
        {
            GetTarget(user);
            timer = walkTime;
        }

        return null;
    }

    private void GetTarget(GameObject user)
    {
        targetObject.transform.position = GetNewPos(user);
    }

    private Vector3 GetNewPos(GameObject user)
    {
        Vector3 newPos = Vector3.zero;

        int l = 10000;

        while (newPos == Vector3.zero || Physics2D.OverlapCircle(newPos, .4f, wallMask))
        {
            l--;
            if (l <= 0)
            {
                Debug.Log("Loop error");
                break;
            }

            if (square)
                newPos = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
            else
                newPos = Random.insideUnitCircle * range;

            if (free)
                newPos += user.transform.position;
            else
                newPos += startPoint;

            if (grid)
            {
                int xTemp = (int)newPos.x;
                int yTemp = (int)newPos.y;

                float x = xTemp + (newPos.x > 0 ? .5f : -.5f);
                float y = yTemp + (newPos.y > 0 ? .5f : -.5f);

                newPos.x = x;
                newPos.y = y;
            }
        }

        return newPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        if (startPoint != Vector3.zero)
        {
            if (square)
                Gizmos.DrawWireCube(startPoint, new Vector3(range*2, range*2, 0));
            else
                Gizmos.DrawWireSphere(startPoint, range);
        }
    }
}
