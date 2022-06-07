using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    public bool isWeapon = false;
    public bool isItem = false;

    public List<Item> items;
    public List<Weapon> weapons;

    public Item item;
    private const float touchCooldown = 2f;

    private float cooldown = 0;

    private void Awake()
    {
        if (isWeapon)
        {
            if (isItem)
            {
                int listNum = Random.Range(0, 2);
                item = listNum == 0 ? weapons[Random.Range(0, weapons.Count)] : items[Random.Range(0, items.Count)];
            }

            item = weapons[Random.Range(0, weapons.Count)];
        }
        else if (isItem)
            item = items[Random.Range(0, items.Count)];

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
            if (tempItem == null)
            {
                Destroy(gameObject);
                return;
            }
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
