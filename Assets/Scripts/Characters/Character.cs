using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    public StatBlock startingStats;
    public List<Weapon> weapons;
    public List<Item> items;

    public StatBlock statBlock;

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

    public int activeWeapon;

    private bool dashing;

    public float health;
    public float mana;

    private float jumpCD;
    private float shootCD;

    private float iFrames = 0;
    public delegate void DoTouch(GameObject col);
    public event DoTouch OnTouch;


    private void Awake()
    {
        InitStats();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weaponObj = transform.Find("WeaponBase").gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();

        activeWeapon = -1;
        dashing = false;
        shooting = false;

        if (weapons.Count > 0)
            ChangeWeapon(0, false);
    }

    private void Start()
    {
        health = statBlock.GetStat("MaxHealth");
        mana = statBlock.GetStat("MaxMana");
    }

    private void Update()
    {
        moveInput = Vector2.zero;

        if (shootCD > 0) shootCD -= Time.deltaTime;
        if (jumpCD > 0) jumpCD -= Time.deltaTime;
        if (iFrames > 0) iFrames -= Time.deltaTime;
        float maxMana = statBlock.GetStat("MaxMana");
        if (mana < maxMana) mana += statBlock.GetStat("ManaRegen") * Time.deltaTime;
        if (mana > maxMana) mana = maxMana;

        SetWeaponPos();
        if (shooting)
        {
            if (shootCD <= 0)
                Shoot();
        }

        anim.SetFloat("VelY", rb.velocity.y);
        anim.SetFloat("Speed", rb.velocity.magnitude);
        anim.SetBool("Dashing", dashing);
        spriteRenderer.flipX = rb.velocity.x < 0 ? true : false;

        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (KeyValuePair<string, float> k in statBlock.stats)
            {
                Debug.Log($"{k.Key}: {k.Value}");
            }
        }
    }

    private void FixedUpdate()
    {
        if (!dashing)
            rb.velocity = moveInput * statBlock.GetStat("MoveSpeed");
    }

    private void InitStats()
    {
        statBlock = ScriptableObject.CreateInstance<StatBlock>();

        if (startingStats != null)
            statBlock.Add(startingStats);

        foreach (Item item in items)
            statBlock.Add(item.statBlock);

        foreach (Weapon weapon in weapons)
            statBlock.Add(weapon.statBlock);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        statBlock.Add(item.statBlock);
    }

    public Weapon AddWeapon(Weapon weapon)
    {
        if (weapons.Count < 2)
        {
            weapons.Add(weapon);
            statBlock.Add(weapon.statBlock);
            if (activeWeapon < 0) ChangeWeapon(0);
            return null;
        }

        Weapon returnWeapon = weapons[activeWeapon];

        statBlock.Remove(weapons[activeWeapon].statBlockHolding);
        weapons[activeWeapon] = weapon;

        ChangeWeapon(activeWeapon, false);

        return returnWeapon;
    }

    public void ChangeWeaponScroll(int dir)
    {
        if (weapons.Count == 0)
        {
            Debug.Log("No weapons in list");
            return;
        }

        int tempActive = activeWeapon + dir;

        while (tempActive < 0 || tempActive >= weapons.Count)
        {
            if (tempActive >= weapons.Count)
                tempActive -= weapons.Count;
            else if (tempActive < 0)
                tempActive += weapons.Count;
        }

        ChangeWeapon(tempActive);
    }

    public void ChangeWeapon(int pos, bool removeOld = true)
    {
        if (pos < weapons.Count)
        {
            if (activeWeapon >= 0 && removeOld)
                statBlock.Remove(weapons[activeWeapon].statBlockHolding);
            activeWeapon = pos;
            if (pos < 0) return;
            statBlock.Add(weapons[activeWeapon].statBlockHolding);
        }
        else
            Debug.Log($"No weapon in slot {pos}");
    }

    private void SetWeaponPos()
    {
        weaponObj.SetActive(shooting);

        if (weapons.Count == 0 || !shooting) return;
        if (activeWeapon < 0)
        {
            weaponObj.SetActive(false);
            return;
        }

        SpriteRenderer sr = weaponObj.GetComponentInChildren<SpriteRenderer>();

        weaponObj.transform.right = lookDir;

        if (sr.sprite != weapons[activeWeapon].weaponSprite)
            sr.sprite = weapons[activeWeapon].weaponSprite;

        sr.flipY = lookDir.x < 0 ? true : false;
        sr.sortingOrder = lookDir.y < 0 ? 1 : -1;
    }

    private void Shoot()
    {
        // TODO: Use various projectile variables

        float manaCost = statBlock.GetStat("ManaCost");
        if (activeWeapon < 0 || mana < manaCost) return;

        mana -= manaCost;

        if (weapons.Count == 0)
        {
            Debug.Log("Pew");
            shootCD = statBlock.GetStat("WeaponCooldown");
            return;
        }

        float projectileCount = statBlock.GetStat("ProjectileCount");
        //Debug.Log(projectileCount);
        if (projectileCount < 1) projectileCount = 1;
        float projectileSpread = statBlock.GetStat("ProjectileSpread");
        //Debug.Log(projectileSpread);


        for (float i = (-projectileCount / 2) + .5f; i < projectileCount / 2; i++)
        {
            GameObject projectile = Instantiate(weapons[activeWeapon].projectile, transform.position + (weaponObj.transform.right * statBlock.GetStat("WeaponOffset")), weaponObj.transform.rotation);
            projectile.transform.Rotate(Vector3.forward * i * projectileSpread);
            projectile.GetComponent<Projectile>().Init(statBlock.GetStat("ProjectileSpeed"), statBlock.GetStat("ProjectileDuration"), (int)statBlock.GetStat("Damage"));
            projectile.layer = gameObject.layer;
        }

        shootCD = statBlock.GetStat("WeaponCooldown");
    }

    public void TakeDamage(int damage)
    {
        if (iFrames > 0) return;
        Debug.Log($"Damage taken: {damage}");
        health -= damage;
        if (health <= 0)
        {
            if (tag == "Player")
                GameOver();
            else if (TryGetComponent(out ItemDrop drops))
                drops.DoDrop();
        }
        iFrames = statBlock.GetStat("IFrames");
    }

    public void Heal(float value)
    {
        health += value;
        if (health > statBlock.GetStat("MaxHealth")) health = statBlock.GetStat("MaxHealth");
    }

    public void MPHeal(float value)
    {
        mana += value;
        if (mana > statBlock.GetStat("MaxMana")) mana = statBlock.GetStat("MaxMana");
    }

    private void GameOver()
    {
        Debug.Log("Player ded");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) return;
        OnTouch?.Invoke(collision.gameObject);
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
        iFrames = statBlock.GetStat("JumpDuration");
        rb.velocity = moveInput * statBlock.GetStat("JumpSpeed");
        yield return new WaitForSeconds(statBlock.GetStat("JumpDuration"));
        dashing = false;
        jumpCD = statBlock.GetStat("JumpCooldown");
        // Jump Animation End
    }
}
