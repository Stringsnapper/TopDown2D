using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public WeaponScriptableObject weaponInstance;

    public GameObject parent;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        parent = transform.parent.gameObject;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Fire()
    {

        if(weaponInstance.ammo > 0)
        {
            for (int i = 0; i < weaponInstance.numberOfProjectiles; i++)
            {

                InstantiateProjectile();

            }
            weaponInstance.ammo--;
        }
        
    }

    public void SetWeaponInstance(WeaponScriptableObject scriptableObject)
    {
        weaponInstance = scriptableObject;
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = weaponInstance.weaponSprite;
        
    }

    private void InstantiateProjectile()
    {
        Vector3 parentRotation = transform.parent.rotation.eulerAngles;
        float angle = Random.Range(weaponInstance.spread * -1, weaponInstance.spread);
        Quaternion rotation = Quaternion.Euler(parentRotation.x, parentRotation.y, parentRotation.z + angle);
        Instantiate<Projectile>(weaponInstance.projectile, transform.position, rotation);
    }
}
