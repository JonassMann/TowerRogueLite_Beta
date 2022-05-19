using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacter : MonoBehaviour
{
    private void Reset()
    {
        gameObject.AddComponent<StateManager>();
        gameObject.AddComponent<Character>();
        gameObject.AddComponent<ItemDrop>();
        gameObject.AddComponent<SpriteRenderer>();
        gameObject.AddComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.AddComponent<Animator>();

        GameObject states = new GameObject("States");
        states.transform.parent = transform;
        states.transform.localPosition = Vector3.zero;

        GameObject newState = new GameObject("New State");
        newState.transform.parent = states.transform;
        newState.transform.localPosition = Vector3.zero;
        newState.AddComponent<State>();
        GetComponent<StateManager>().currentState = newState.GetComponent<State>();

        //GameObject hitbox = new GameObject("Hitbox");
        //hitbox.transform.parent = transform;
        //hitbox.transform.localPosition = Vector3.zero;
        //hitbox.tag = "Hitbox";
        //hitbox.layer = gameObject.layer;
        //hitbox.AddComponent<CircleCollider2D>();

        GameObject weaponBase = new GameObject("WeaponBase");
        weaponBase.transform.parent = transform;
        weaponBase.transform.localPosition = Vector3.zero;

        GameObject weapon = new GameObject("Weapon");
        weapon.transform.parent = weaponBase.transform;
        weapon.transform.localPosition = new Vector3(1, 0, 0);
        weapon.AddComponent<SpriteRenderer>();

        DestroyImmediate(this);
    }
}
