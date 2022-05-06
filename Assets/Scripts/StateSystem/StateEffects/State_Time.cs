using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Time : StateEffect
{
    public State state;
    public float timeSec = 0;

    private float timer;

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget)
    {
    }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget)
    {
        timer = timeSec;
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        timer -= Time.deltaTime;

        if (timer <= 0) return state;
        return null;
    }
}
