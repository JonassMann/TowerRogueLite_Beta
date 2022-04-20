using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public StatBlock startingStats;
    public List<Weapon> weapons;
    public List<Item> items;

    private StatBlock statBlock;

    private Rigidbody2D rb;
    private Animator anim;
    private GameObject weaponObj;
    private SpriteRenderer spriteRenderer;

    [HideInInspector]
    public Vector2 moveInput;
    [HideInInspector]
    public bool shooting;
    [HideInInspector]
    public Vector2 lookDir;

    private int activeWeapon;

    private bool dashing;

    private int health;
    private int mana;

    private float jumpCD;
    private float shootCD;

    private void Awake()
    {
        InitStats();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weaponObj = transform.Find("WeaponBase").gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();

        dashing = false;
        shooting = false;

        health = (int)statBlock.GetStat("HealthMax");

        ChangeWeapon(0);
    }

    private void Update()
    {
        moveInput = Vector2.zero;

        if (shootCD > 0) shootCD -= Time.deltaTime;
        if (jumpCD > 0) jumpCD -= Time.deltaTime;

        if (shooting && shootCD <= 0) Shoot();

        anim.SetFloat("VelY", rb.velocity.y);
        anim.SetFloat("Speed", rb.velocity.magnitude);
        anim.SetBool("Dashing", dashing);
        spriteRenderer.flipX = rb.velocity.x < 0 ? true : false;

        SetWeaponPos();
    }

    private void FixedUpdate()
    {
        if (!dashing)
            rb.velocity = moveInput * statBlock.GetStat("MoveSpeed", GetActiveWeaponStats());
    }

    private void InitStats()
    {
        statBlock = ScriptableObject.CreateInstance<StatBlock>();

        if (startingStats != null)
            statBlock.Add(startingStats);

        foreach (Item item in items)
            statBlock.Add(item.statBlock);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        statBlock.Add(item.statBlock);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        statBlock.Remove(item.statBlock);
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }

    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
    }

    private StatBlock GetActiveWeaponStats()
    {
        if (weapons.Count == 0 || activeWeapon >= weapons.Count) return null;
        return weapons[activeWeapon].statBlock;
    }

    public void ChangeWeaponScroll(int dir)
    {
        if (weapons.Count == 0)
        {
            Debug.Log("No weapons in list");
            return;
        }

        activeWeapon += dir;
        while (activeWeapon < 0 || activeWeapon >= weapons.Count)
        {
            if (activeWeapon >= weapons.Count)
                activeWeapon -= weapons.Count;
            else if (activeWeapon < 0)
                activeWeapon += weapons.Count;
        }
    }

    public void ChangeWeapon(int pos)
    {
        if (pos < weapons.Count)
        {
            activeWeapon = pos;
        }
        else
            Debug.Log($"No weapon in slot {pos}");
    }

    private void SetWeaponPos()
    {
        if (weapons.Count == 0) return;

        SpriteRenderer sr = weaponObj.GetComponentInChildren<SpriteRenderer>();

        weaponObj.transform.right = lookDir;

        if (sr.sprite != weapons[activeWeapon].weaponSprite)
            sr.sprite = weapons[activeWeapon].weaponSprite;

        sr.flipY = lookDir.x < 0 ? true : false;
        sr.sortingOrder = lookDir.y < 0 ? 1 : -1;

        weaponObj.SetActive(shooting);
    }

    private void Shoot()
    {
        // TODO: Use various projectile variables

        if (weapons.Count == 0)
        {
            Debug.Log("Pew");
            shootCD = statBlock.GetStat("WeaponCooldown", GetActiveWeaponStats());
            return;
        }

        float projectileCount = statBlock.GetStat("ProjectileCount", GetActiveWeaponStats());
        if (projectileCount < 1) projectileCount = 1;
        float projectileSpread = statBlock.GetStat("ProjectileSpread", GetActiveWeaponStats());

        for (float i = (-projectileCount / 2) + .5f; i < projectileCount / 2; i++)
        {
            GameObject projectile = Instantiate(weapons[activeWeapon].projectile, transform.position + (weaponObj.transform.right * statBlock.GetStat("WeaponOffset", GetActiveWeaponStats())), weaponObj.transform.rotation);
            projectile.transform.Rotate(Vector3.forward * i * projectileSpread);
            projectile.GetComponent<Projectile>().Init(statBlock.GetStat("ProjectileSpeed", GetActiveWeaponStats()), statBlock.GetStat("ProjectileDuration", GetActiveWeaponStats()), (int)statBlock.GetStat("Damage", GetActiveWeaponStats()));
            projectile.layer = gameObject.layer;
        }

        shootCD = statBlock.GetStat("WeaponCooldown", GetActiveWeaponStats());
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameObject.tag == "Player")
            {
                if (health <= 0)
                {
                    Debug.Log("Player Dead");
                }
            }
            else if (transform.parent.name == "Spawner")
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    public void JumpStart()
    {
        if (jumpCD <= 0 && rb.velocity != Vector2.zero)
            StartCoroutine(DoJump());
    }

    private IEnumerator DoJump()
    {
        dashing = true;
        // Jump Animation Start
        transform.Find("Hitbox").GetComponent<Collider2D>().enabled = false;
        rb.velocity = moveInput * statBlock.GetStat("JumpSpeed", GetActiveWeaponStats());
        yield return new WaitForSeconds(statBlock.GetStat("JumpDuration", GetActiveWeaponStats()));
        dashing = false;
        jumpCD = statBlock.GetStat("JumpCooldown", GetActiveWeaponStats());
        // Jump Animation End
        transform.Find("Hitbox").GetComponent<Collider2D>().enabled = true;
    }
}
