using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    Character player;

    public Image selectedWeapon;

    public List<Sprite> selectedSprites;

    public Image leftWeapon;
    public Image rightWeapon;

    private int selected;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    private void Start()
    {
        selected = player.activeWeapon;
    }

    private void Update()
    {
        if (player.activeWeapon != selected)
        {
            selected = player.activeWeapon;
            selectedWeapon.sprite = selectedSprites[selected - 1];
        }

        leftWeapon.sprite = player.weapons.Count > 0 ? player.weapons[0].sprite : null;
        rightWeapon.sprite = player.weapons.Count > 1 ? player.weapons[1].sprite : null;
    }
}
