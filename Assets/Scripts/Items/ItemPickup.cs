using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    public Item item;
    private const float touchCooldown = 2f;

    private float cooldown = 0;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }

    private void Update()
    {
        if (cooldown > 0) cooldown -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" || cooldown > 0) return;
        cooldown = touchCooldown;

        if (item is Weapon)
        {
            Item tempItem = collision.gameObject.GetComponent<Character>().AddWeapon(item as Weapon);
            if (tempItem == null) Destroy(gameObject);
            item = tempItem;
            GetComponent<SpriteRenderer>().sprite = item.sprite;
        }
        else
        {
            collision.gameObject.GetComponent<Character>().AddItem(item);
            Destroy(gameObject);
        }
    }
}
