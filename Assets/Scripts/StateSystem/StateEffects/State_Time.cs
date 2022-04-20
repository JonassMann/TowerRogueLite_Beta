using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Time : StateEffect
{
    public State state;
    public float timer = 0;

    public override void OnEnd(GameObject user, GameObject target)
    {
    }

    public override void OnStart(GameObject user, GameObject target)
    {
    }

    public override State OnUpdate(GameObject user, GameObject target)
    {
        timer -= Time.deltaTime;

        if (timer <= 0) return state;
        return null;
    }
}
