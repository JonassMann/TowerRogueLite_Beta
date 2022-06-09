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

    private float showWeaponTimer;

    public int activeWeapon;

    private bool dashing;

    public float health;
    public float mana;

    private float jumpCD;
    private float shootCD;

    private float iFrames = 0;
    public delegate void DoTouch(GameObject col);
    public event DoTouch OnTouch;

    [HideInInspector]
    public Tarot tarots;

    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource itemCollect;

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
        if (Time.timeScale == 0) return;

        if (shootCD > 0) shootCD -= Time.deltaTime;
        if (jumpCD > 0) jumpCD -= Time.deltaTime;
        if (iFrames > 0) iFrames -= Time.deltaTime;
        float maxMana = statBlock.GetStat("MaxMana");
        float manaRegen = statBlock.GetStat("ManaRegen");
        manaRegen *= tarots.HasFlag(Tarot.TheEmpress) ? 2 : 1;
        manaRegen *= tarots.HasFlag(Tarot.Death) ? .5f : 1;
        manaRegen *= tarots.HasFlag(Tarot.TheStar) ? 2 : 1;
        manaRegen *= tarots.HasFlag(Tarot.TheMoon) ? 2 : 1;

        if (mana < maxMana) mana += manaRegen * Time.deltaTime;
        if (mana > maxMana) mana = maxMana;

        SetWeaponPos();
        if (shooting)
        {
            showWeaponTimer = 1f;
            if (shootCD <= 0)
                Shoot();
        }
        else if (showWeaponTimer > 0) showWeaponTimer -= Time.deltaTime;

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
        {
            float moveSpeed = statBlock.GetStat("MoveSpeed");
            moveSpeed *= tarots.HasFlag(Tarot.TheChariot) ? 2 : 1;
            moveSpeed *= tarots.HasFlag(Tarot.TheHermit) ? .5f : 1;
            moveSpeed *= tarots.HasFlag(Tarot.TheDevil) ? .5f : 1;

            rb.velocity = moveInput * moveSpeed;
        }
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

    public void ResetTarot()
    {
        tarots = Tarot.None;
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
            //Debug.Log(gameObject.name);
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
        weaponObj.SetActive(showWeaponTimer > 0 ? true : shooting);

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

        float manaCost = tarots.HasFlag(Tarot.TheHighPriestess) ? 0 : statBlock.GetStat("ManaCost");
        if (activeWeapon < 0 || mana < manaCost) return;

        mana -= manaCost;

        if (weapons.Count == 0)
        {
            Debug.Log("Pew");
            shootCD = statBlock.GetStat("WeaponCooldown");
            return;
        }

        float projectileCount = statBlock.GetStat("ProjectileCount");
        if (projectileCount < 1) projectileCount = 1;
        float projectileSpread = statBlock.GetStat("ProjectileSpread");
        if (projectileSpread == 0) projectileSpread = 10;
        int damageMultiplier = 1;
        float projectileCountMulti = 1;
        float projectileSpeedMulti = 1;

        damageMultiplier *= tarots.HasFlag(Tarot.TheEmperor) ? 2 : 1;
        damageMultiplier *= tarots.HasFlag(Tarot.TheHangedMan) ? 2 : 1;
        damageMultiplier *= tarots.HasFlag(Tarot.TheTower) ? 2 : 1;

        projectileSpeedMulti *= tarots.HasFlag(Tarot.TheHierophant) ? 3 : 1;
        projectileSpeedMulti *= tarots.HasFlag(Tarot.TheDevil) ? 2 : 1;

        projectileCountMulti *= tarots.HasFlag(Tarot.TheHermit) ? 2 : 1;
        projectileCountMulti *= tarots.HasFlag(Tarot.TheTower) ? 3 : 1;

        projectileCount *= projectileCountMulti;
        for (float i = (-projectileCount / 2) + .5f; i < projectileCount / 2; i++)
        {
            GameObject projectile = Instantiate(weapons[activeWeapon].projectile, transform.position + (weaponObj.transform.right * statBlock.GetStat("WeaponOffset")), weaponObj.transform.rotation);
            projectile.transform.Rotate(Vector3.forward * i * projectileSpread);
            projectile.GetComponent<Projectile>().Init(statBlock.GetStat("ProjectileSpeed") * projectileSpeedMulti, statBlock.GetStat("ProjectileDuration"), (int)statBlock.GetStat("Damage") * damageMultiplier);
            projectile.layer = gameObject.layer;
        }

        shootCD = statBlock.GetStat("WeaponCooldown");
        if (tarots.HasFlag(Tarot.Temperance)) shootCD /= 2;
    }

    public void TakeDamage(int damage)
    {
        if (iFrames > 0 || tarots.HasFlag(Tarot.TheLovers) || health <= 0) return;
        // Debug.Log($"Damage taken: {damage}");
        if (tarots.HasFlag(Tarot.TheHangedMan)) damage *= 2;
        if (tarots.HasFlag(Tarot.Judgement)) damage *= 2;
        if (tarots.HasFlag(Tarot.TheTower)) damage *= 2;
        if (tarots.HasFlag(Tarot.Justice)) damage = 1;
        if (tarots.HasFlag(Tarot.TheSun)) damage = 0;

        health -= damage;

        //Sound on Hit

        if(health > 0 && hitSound != null)
            hitSound.Play();


        //Debug.Log($"{gameObject.name} hp: {health}");

        if (health <= 0)
        {
            if(deathSound != null)
                deathSound.Play();

            if (tag == "Player")
                GameOver();
            else
            {
                if(name == "Boss") GameObject.Find("EndMenu").GetComponent<EndScene>().GameEnd("Win");
                transform.parent.parent.GetComponent<RoomActive>().CheckCount(gameObject);
            }
        }
        iFrames = statBlock.GetStat("IFrames");
    }

    public void Heal(float value)
    {
        if (itemCollect != null)
            itemCollect.Play();

        float healthAdd = value;
        healthAdd *= tarots.HasFlag(Tarot.Strength) ? 2 : 1;
        healthAdd *= tarots.HasFlag(Tarot.TheStar) ? 2 : 1;

        health += healthAdd;

        float maxHealth = statBlock.GetStat("MaxHealth");
        maxHealth *= tarots.HasFlag(Tarot.Death) ? .5f : 1;
        maxHealth *= tarots.HasFlag(Tarot.TheTower) ? .5f : 1;

        if (health > maxHealth) health = maxHealth;
    }

    public void MPHeal(float value)
    {
        if (itemCollect != null)
            itemCollect.Play();

        float manaAdd = value;
        manaAdd *= tarots.HasFlag(Tarot.TheMagician) ? 2 : 1;

        mana += manaAdd;
        if (mana > statBlock.GetStat("MaxMana")) mana = statBlock.GetStat("MaxMana");
    }

    private void GameOver()
    {
        if (tarots.HasFlag(Tarot.TheFool))
        {
            Debug.Log("Fool trigger");
            health = 1;
            tarots &= ~Tarot.TheFool;
            return;
        }

        GameObject.Find("EndMenu").GetComponent<EndScene>().GameEnd("Lose");
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
        dashSound.Play();
        // Jump Animation Start
        iFrames = statBlock.GetStat("JumpDuration");
        rb.velocity = moveInput * statBlock.GetStat("JumpSpeed");
        yield return new WaitForSeconds(statBlock.GetStat("JumpDuration"));
        dashing = false;
        jumpCD = statBlock.GetStat("JumpCooldown");
        // Jump Animation End
    }
}
