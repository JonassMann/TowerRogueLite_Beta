using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash_Input : StateEffect
{
    private PlayerInputActions playerInputActions;
    private Character user;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.Jump.performed += Jump_performed;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Jump.performed -= Jump_performed;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        user.JumpStart();
    }

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget) { }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget)
    {
        this.user = user.GetComponent<Character>();
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget) 
    {
        return null;
    }
}
