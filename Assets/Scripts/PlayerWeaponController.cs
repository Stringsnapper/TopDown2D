using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    private Weapon[] weapons;

    [SerializeField]
    private WeaponScriptableObject[] weaponScriptableObjects;

    private List<WeaponScriptableObject> weaponInstances;

    private int currentWeaponIndex = 0;

    private GameObject body;
    private SpriteRenderer bodySpriteRenderer;

    // Start is called before the first frame update

    void Start()
    {
        PopulateWeaponInstances();
        body = transform.Find("Body").gameObject;
        bodySpriteRenderer = body.GetComponent<SpriteRenderer>();
        SelectWeapon(currentWeaponIndex);
    }

    public void OnFire(InputAction.CallbackContext obj)
    {
        Debug.Log("Firing");
        if (obj.phase == InputActionPhase.Started)
        {
            weapon.Fire();
        }
    }

    public void OnWeaponSelect(InputAction.CallbackContext context)
    {
        
        float selectDirection = context.ReadValue<float>();
        if (context.phase == InputActionPhase.Performed && selectDirection != 0)
        {
            if (selectDirection > 0)
            {
                currentWeaponIndex += 1;
            }
            if (selectDirection < 0)
            {
                currentWeaponIndex -= 1;
            }

            currentWeaponIndex = Mathf.Clamp(currentWeaponIndex, 0, weaponInstances.Count - 1);
            SelectWeapon(currentWeaponIndex);
        }
    }

    private void PopulateWeaponInstances()
    {
        if(weaponInstances == null || weaponInstances.Count == 0)
        {
            weaponInstances = new List<WeaponScriptableObject>();
            foreach (var so in weaponScriptableObjects)
            {
                var instance = Instantiate(so);
                weaponInstances.Add(instance);
            }
        }
    }

    private void SelectWeapon(int weaponIndex)
    {
        weapon.SetWeaponInstance(weaponInstances[weaponIndex]);
        bodySpriteRenderer.sprite = weapon.weaponInstance.playerBodySprite;
    }
}
