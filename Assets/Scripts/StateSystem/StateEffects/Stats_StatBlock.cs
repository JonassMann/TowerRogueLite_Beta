using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stats_StatBlock : StateEffect
{
    public StatBlock statBlock;

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget)
    {
        user.GetComponent<Character>().statBlock.Remove(statBlock);
    }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget)
    {
        user.GetComponent<Character>().statBlock.Add(statBlock);
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        return null;
    }
}
