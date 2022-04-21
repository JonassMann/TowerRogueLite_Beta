using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Time : StateEffect
{
    public State state;
    public float timeSec = 0;

    private float timer;

    public override void OnEnd(GameObject user, GameObject target)
    {
    }

    public override void OnStart(GameObject user, GameObject target)
    {
        timer = timeSec;
    }

    public override State OnUpdate(GameObject user, GameObject target)
    {
        timer -= Time.deltaTime;

        if (timer <= 0) return state;
        return null;
    }
}
