using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot_Weapon : StateEffect
{
    public int selectedWeapon = 0;

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget)
    {
        user.GetComponent<Character>().ChangeWeapon(-1);
        user.GetComponent<Character>().shooting = false;
    }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget)
    {
        user.GetComponent<Character>().lookDir = target.transform.position - user.transform.position;
        user.GetComponent<Character>().ChangeWeapon(selectedWeapon);
        user.GetComponent<Character>().shooting = true;
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        user.GetComponent<Character>().lookDir = target.transform.position - user.transform.position;

        return null;
    }
}
