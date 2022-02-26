using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("WeaponToSpawn"), SerializeField]
    private GameObject weaponToSpawn;

    public Sprite crossHairImage;

    [SerializeField]
    private GameObject weaponSocket;
    [SerializeField]
    private Transform gripIKSocket;
    [SerializeField]
    private WeaponComponent equippedWeapon;

    #region Component Reference Variables
    private PlayerController playerController;
    public PlayerController controller => playerController;
    private Animator animator;
    #endregion

    private bool firingPressed = false;

    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    void Start()
    {
        GameObject spawnedWeaponGO = Instantiate(weaponToSpawn, weaponSocket.transform.position, weaponSocket.transform.rotation, weaponSocket.transform);
        WeaponComponent spawnedWeapon = spawnedWeaponGO.GetComponent<WeaponComponent>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        EquipWeapon(spawnedWeapon);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!playerController.isReloading && equippedWeapon && equippedWeapon.gripLocation)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, gripIKSocket.transform.position);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        }
    }

    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;
        StartReloading();
    }

    public void StartReloading()
    {
        if (playerController.isFiring)
        {
            StopFiring();
        }
        if (equippedWeapon.stats.totalBullets <= 0)
        {
            return;
        }

        // Refill ammo here
        if (!equippedWeapon.ShouldReload()) return;
        equippedWeapon.StartReloading();

        playerController.isReloading = true;
        animator.SetBool(isReloadingHash, true);
        InvokeRepeating(nameof(StopReloading), 0, 0.1f);
    }

    private void StopReloading()
    {
        if (animator.GetBool(isReloadingHash)) return;
        
        playerController.isReloading = false;
        equippedWeapon.StopReloading();
        CancelInvoke(nameof(StopReloading));

        if (firingPressed)
        {
            StartFiring();
        }
    }

    public void OnFire(InputValue value)
    {
        firingPressed = value.isPressed;
        if (firingPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }
    }

    private void StartFiring()
    {
        if (equippedWeapon.stats.bulletsInClip <= 0)
        {
            StartReloading();
            return;
        }

        playerController.isFiring = true;
        animator.SetBool(isFiringHash, playerController.isFiring);
        equippedWeapon.StartFiring();
    }

    private void StopFiring()
    {
        playerController.isFiring = false;
        animator.SetBool(isFiringHash, playerController.isFiring);
        equippedWeapon.StopFiring();
    }

    public void EquipWeapon(WeaponComponent weaponComponent)
    {
        equippedWeapon = weaponComponent;
        equippedWeapon.weaponHolder = this;
        gripIKSocket = equippedWeapon.gripLocation;
        PlayerEvents.InvokeOnWeaponEquipped(weaponComponent);
    }
}
