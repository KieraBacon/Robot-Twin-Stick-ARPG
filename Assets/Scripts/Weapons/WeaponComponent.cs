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
    public int bulletsInClip;
    public int clipSize;
    public int totalBullets;
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
        Debug.Log("Firing weapon! " + stats.bulletsInClip + " bullets left in clip.");
    }

    public virtual bool ShouldReload()
    {
        return stats.totalBullets > 0 && (stats.dumpAmmoOnReload || stats.bulletsInClip < stats.clipSize);
    }

    public virtual void StartReloading()
    {
        if (stats.totalBullets > 0)
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

        int bulletsToFillClip = stats.dumpAmmoOnReload ? stats.clipSize : stats.clipSize - stats.bulletsInClip;
        int bulletsLeftAfter = stats.totalBullets - bulletsToFillClip;

        if (bulletsLeftAfter >= 0)
        {
            stats.totalBullets -= bulletsToFillClip;
            stats.bulletsInClip = stats.clipSize;
        }
        else
        {
            stats.bulletsInClip += stats.totalBullets;
            stats.totalBullets = 0;
        }
    }
}
