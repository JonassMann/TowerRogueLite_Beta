using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Distance : StateEffect
{
    public State state;
    public GameObject target;
    public bool close;
    public float distance;

    public override void OnEnd(GameObject user, GameObject target)
    {
    }

    public override void OnStart(GameObject user, GameObject target)
    {
    }

    public override State OnUpdate(GameObject user, GameObject target)
    {
        if(this.target != null && (Vector3.Distance(user.transform.position, this.target.transform.transform.position) < distance) == close) return state;
        if(target != null && (Vector3.Distance(user.transform.position, target.transform.transform.position) < distance) == close) return state;

        return null;
    }
}
