using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None = 0,
    MachineGun = 1,
    Sword = 2,
    Fist = 3,
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
    public WeaponStats stats => scriptable.weaponStats;
    public WeaponScriptable scriptable;
    public WeaponHolder weaponHolder;
    [SerializeField]
    protected ParticleSystem firingEffect;

    protected bool isFiring;
    protected bool isReloading;
    
    protected Camera mainCamera;

    private float nextFireTime;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        DrawAimTelegraph();
        if (nextFireTime > 0 && nextFireTime - Time.time < 0)
        {
            if (isFiring)
            {
                Fire();
            }
            else
            {
                Debug.Log("Finishing the firing sequence!");
                weaponHolder.FinishFiring();
                nextFireTime = -1;
            }
        }
    }

    public virtual void StartFiring()
    {
        if (isFiring)
        {
            Debug.Log("Is already firing!");
            return;
        }
        else if (nextFireTime - Time.time < 0) // There's still time remaining after the last shot before the next one can fire
        {
            if (stats.fireStartDelay > 0)
            {
                Debug.Log("Was not firing, but there was a fire start delay!");
                nextFireTime = Time.time + stats.fireStartDelay;
            }
            else
            {
                Debug.Log("Was not firing, and now is!");
                Fire();
            }
        }

        isFiring = true;
    }

    public virtual void StopFiring()
    {
        isFiring = false;
        if (firingEffect && firingEffect.isPlaying)
            firingEffect.Stop();
    }

    protected virtual void Fire()
    {
        nextFireTime = Time.time + stats.refireRate;
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

        if (ammoItem.amount <= 0)
        {
            ammoItem.DeleteItem(weaponHolder.controller);
            weaponHolder.controller.inventory.DeleteItem(ammoItem);
        }
    }
}
