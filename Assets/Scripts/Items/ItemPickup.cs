using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    public bool isWeapon = false;

    public List<Item> items;
    public List<Weapon> weapons;

    private Item item;
    private const float touchCooldown = 2f;

    private float cooldown = 0;

    [SerializeField] private AudioSource weaponPickUpSound;
    

    private void Awake()
    {
        item = isWeapon ? weapons[Random.Range(0, weapons.Count)] : items[Random.Range(0, items.Count)];

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

        weaponPickUpSound.Play();

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
