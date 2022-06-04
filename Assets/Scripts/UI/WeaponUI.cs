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

    private int selected = -1;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    private void Update()
    {
        if (player.activeWeapon != selected)
        {
            selected = player.activeWeapon;
            selectedWeapon.sprite = selectedSprites[selected + 1];
        }

        if (player.weapons.Count > 0)
        {
            rightWeapon.gameObject.SetActive(true);
            rightWeapon.sprite = player.weapons[0].sprite;
        }
        else
            rightWeapon.gameObject.SetActive(false);

        if (player.weapons.Count > 1)
        {
            leftWeapon.gameObject.SetActive(true);
            leftWeapon.sprite = player.weapons[1].sprite;
        }
        else
            leftWeapon.gameObject.SetActive(false);
    }
}
