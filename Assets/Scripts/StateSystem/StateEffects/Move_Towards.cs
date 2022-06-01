using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Towards : StateEffect
{
    public float weight = 1;
    public GameObject target;

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget) { }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget) { }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        if (this.target != null) user.GetComponent<Character>().moveInput += (Vector2)(this.target.transform.position - user.transform.position).normalized * weight;
        else if(moveTarget != null) user.GetComponent<Character>().moveInput += (Vector2)(moveTarget.transform.position - user.transform.position).normalized * weight;

        return null;
    }
}
