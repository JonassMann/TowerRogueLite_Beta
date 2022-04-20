using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacter : MonoBehaviour
{
    private void Reset()
    {
        gameObject.AddComponent<StateManager>();
        gameObject.AddComponent<Character>();
        gameObject.AddComponent<SpriteRenderer>();
        gameObject.AddComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.AddComponent<Animator>();

        GameObject states = new GameObject("States");
        states.transform.parent = transform;

        GameObject newState = new GameObject("New State");
        newState.transform.parent = states.transform;
        newState.AddComponent<State>();

        GameObject hitbox = new GameObject("Hitbox");
        hitbox.transform.parent = transform;
        hitbox.tag = "Hitbox";
        hitbox.layer = gameObject.layer;
        hitbox.AddComponent<CircleCollider2D>();

        GameObject weaponBase = new GameObject("WeaponBase");
        weaponBase.transform.parent = transform;

        GameObject weapon = new GameObject("Weapon");
        weapon.transform.parent = weaponBase.transform;
        weapon.transform.position = new Vector3(1, 0, 0);
        weapon.AddComponent<SpriteRenderer>();

        DestroyImmediate(this);
    }
}
