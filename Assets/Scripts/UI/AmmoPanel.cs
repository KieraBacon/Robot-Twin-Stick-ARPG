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

    private void OnEnable()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponEquipped;
    }

    private void OnDisable()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponEquipped;
    }

    private void OnWeaponEquipped(WeaponComponent weaponComponent)
    {
        this.weaponComponent = weaponComponent;
    }

    private void Update()
    {
        if (!weaponComponent)
            return;

        weaponNameText.text = weaponComponent.stats.weaponName;
        currentAmmoText.text = weaponComponent.stats.bulletsInClip.ToString();
        totalAmmoText.text = weaponComponent.stats.totalBullets.ToString();
    }
}
