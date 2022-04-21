using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_LineOfSight : StateEffect
{
    public State state;
    public GameObject target;
    public bool gain;

    private LayerMask wall;

    private void Awake()
    {
        wall |= (1 << LayerMask.NameToLayer("Wall"));
    }

    public override void OnEnd(GameObject user, GameObject target)
    {
    }

    public override void OnStart(GameObject user, GameObject target)
    {
    }

    public override State OnUpdate(GameObject user, GameObject target)
    {
        if (this.target != null && (Physics2D.Linecast(user.transform.position, this.target.transform.position, wall) != gain)) return state;
        if (target != null && (Physics2D.Linecast(user.transform.position, target.transform.position, wall) != gain)) return state;

        return null;
    }
}
