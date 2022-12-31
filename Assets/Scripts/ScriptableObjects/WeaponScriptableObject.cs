using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponScriptableObject", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public Sprite weaponSprite;
    public Sprite playerBodySprite;
    public Projectile projectile;
    public int numberOfProjectiles;
    public float spread;
    public int ammo;

}
