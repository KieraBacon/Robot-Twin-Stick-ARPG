using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI weaponNameText;
    [SerializeField]
    private TextMeshProUGUI currentAmmoText;
    [SerializeField]
    private TextMeshProUGUI totalAmmoText;
    private WeaponComponent weaponComponent;

    private void Start()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponEquipped;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponEquipped;
    }

    private void OnWeaponEquipped(WeaponComponent weaponComponent)
    {
        this.weaponComponent = weaponComponent;
    }

    private void Update()
    {
        if (weaponComponent)
        {
            weaponNameText.text = weaponComponent.stats.weaponName;
            currentAmmoText.text = weaponComponent.weaponHolder.bulletsInClips[weaponComponent.stats.weaponName].ToString();
            ItemScriptable ammoItem = weaponComponent.weaponHolder.player.inventory.FindItem(weaponComponent.stats.weaponName + " Ammo");
            totalAmmoText.text = ammoItem ? ammoItem.amount.ToString() : "0";
        }
        else
        {
            weaponNameText.text = "";
            currentAmmoText.text = "";
            totalAmmoText.text = "";
        }
    }
}
