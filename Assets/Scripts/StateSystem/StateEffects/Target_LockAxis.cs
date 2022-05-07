using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_LockAxis : StateEffect
{
    public bool lockX;
    public bool lockY;

    private Character character;

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget) { }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget) 
    {
        character = user.GetComponent<Character>();
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        Vector2 moveInput = character.moveInput;
        float length = moveInput.magnitude;

        if (lockX) moveInput *= new Vector2(0, 1);
        if (lockY) moveInput *= new Vector2(1, 0);

        character.moveInput = moveInput.normalized * length;

        return null;
    }
}
