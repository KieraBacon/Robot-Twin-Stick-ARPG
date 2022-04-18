using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None, Pistol, MachineGun,
}

public enum WeaponFiringPattern
{
    SemiAuto, FullAuto, ThreeShotBurst, FiveShotBurst,
}

[System.Serializable]
public struct WeaponStats
{
    public WeaponType type;
    public WeaponFiringPattern firingPattern;
    public string weaponName;
    public float damage;
    public int clipSize;
    public float fireStartDelay;
    public float refireRate;
    public float range;
    public bool repeating;
    public LayerMask weaponHitLayers;
    public bool dumpAmmoOnReload;
    public bool fireWhileMoving;
}

public class WeaponComponent : MonoBehaviour
{
    [SerializeField]
    private Transform _gripLocation;
    public Transform gripLocation => _gripLocation;
    [SerializeField]
    private Transform _muzzleLocation;
    public Transform muzzleLocation => _muzzleLocation;
    public WeaponStats stats;
    public WeaponHolder weaponHolder;
    [SerializeField]
    protected ParticleSystem firingEffect;

    protected bool isFiring;
    protected bool isReloading;
    
    protected Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        DrawAimTelegraph();
    }

    public virtual void StartFiring()
    {
        isFiring = true;
        if (stats.repeating)
        {
            var n = nameof(Fire);
            CancelInvoke(n);
            InvokeRepeating(n, stats.fireStartDelay, stats.refireRate);
        }
        else
        {
            Fire();
        }
    }

    public virtual void StopFiring()
    {
        isFiring = false;
        CancelInvoke(nameof(Fire));
        if (firingEffect && firingEffect.isPlaying)
            firingEffect.Stop();
    }

    protected virtual void Fire()
    {
        Debug.Log("Firing weapon! " + weaponHolder.bulletsInClips[stats.weaponName] + " bullets left in clip.");
    }

    protected virtual void DrawAimTelegraph() { }

    public virtual bool ShouldReload()
    {
        ItemScriptable ammoItem = weaponHolder.player.inventory.FindItem(stats.weaponName + " Ammo");
        return ammoItem && ammoItem.amount > 0 && (stats.dumpAmmoOnReload || weaponHolder.bulletsInClips[stats.weaponName] < stats.clipSize);
    }

    public virtual void StartReloading()
    {
        ItemScriptable ammoItem = weaponHolder.player.inventory.FindItem(stats.weaponName + " Ammo");
        if (ammoItem && ammoItem.amount > 0)
        {
            isReloading = true;
            ReloadWeapon();
        }
    }

    public virtual void StopReloading()
    {
        isReloading = false;
    }

    // Set ammo counts here.
    protected virtual void ReloadWeapon()
    {
        // Check to see if there is if there is a firing effect and stop it.
        if (firingEffect && firingEffect.isPlaying)
            firingEffect.Stop();

        ItemScriptable ammoItem = weaponHolder.player.inventory.FindItem(stats.weaponName + " Ammo");
        int bulletsToFillClip = stats.dumpAmmoOnReload ? stats.clipSize : stats.clipSize - weaponHolder.bulletsInClips[stats.weaponName];
        int bulletsLeftAfter = ammoItem.amount - bulletsToFillClip;

        if (bulletsLeftAfter >= 0)
        {
            ammoItem.amount -= bulletsToFillClip;
            weaponHolder.bulletsInClips[stats.weaponName] = stats.clipSize;
        }
        else
        {
            weaponHolder.bulletsInClips[stats.weaponName] += ammoItem.amount;
            ammoItem.amount = 0;
        }
    }
}
