using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public string description;

    public Sprite weaponSprite;

    public GameObject projectile;

    public StatBlock statBlock;

    public StatBlock statBlockHolding;
}
