using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot_Input : StateEffect
{
    private PlayerInputActions playerInputActions;
    private Character user;

    private Camera cam;
    private Vector2 mousePos;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.Shoot.started += Shoot_performed;
        playerInputActions.Player.Shoot.canceled += Shoot_performed;
        playerInputActions.Player.ChangeWeapon.performed += ChangeWeapon_performed;
        cam = Camera.main;
    }

    private void ChangeWeapon_performed(InputAction.CallbackContext obj)
    {
        //user.GetComponent<Character>().ChangeWeaponScroll(obj.ReadValue<float>() > 0 ? 1 : -1);
        if(user == null) playerInputActions.Player.ChangeWeapon.performed -= ChangeWeapon_performed;
        user.GetComponent<Character>().ChangeWeapon(Mathf.Clamp((int)obj.ReadValue<float>(), 0, 1));
    }

    private void OnDisable()
    {
        playerInputActions.Player.Shoot.started -= Shoot_performed;
        playerInputActions.Player.Shoot.canceled -= Shoot_performed;
    }

    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        user.shooting = obj.ReadValue<float>() == 0 ? false : true;
    }

    public override void OnEnd(GameObject user, GameObject target, GameObject moveTarget) { }

    public override void OnStart(GameObject user, GameObject target, GameObject moveTarget)
    {
        this.user = user.GetComponent<Character>();
    }

    public override State OnUpdate(GameObject user, GameObject target, GameObject moveTarget)
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        this.user.lookDir = (Vector3)mousePos - user.transform.position;

        return null;
    }
}
