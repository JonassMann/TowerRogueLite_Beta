using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Input : StateEffect
{
    private PlayerInputActions playerInputActions;
    public float weight = 1;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget) { }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget) { }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        user.GetComponent<Character>().moveInput += playerInputActions.Player.Movement.ReadValue<Vector2>() * weight;

        return null;
    }
}
