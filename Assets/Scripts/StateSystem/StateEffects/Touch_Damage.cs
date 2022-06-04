using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Damage : StateEffect
{
    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget)
    {
        user.GetComponent<Character>().OnTouch -= OnTouch;
    }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget)
    {
        user.GetComponent<Character>().OnTouch += OnTouch;
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        return null;
    }

    private void OnTouch(GameObject col)
    {
        if (col.tag != "Player") return;
        col.GetComponent<Character>().TakeDamage((int)transform.parent.parent.GetComponent<Character>().statBlock.GetStat("Damage"));
    }
}
