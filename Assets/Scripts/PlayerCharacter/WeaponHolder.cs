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
    public WeaponComponent equippedWeapon;

    #region Component Reference Variables
    private PlayerController playerController;
    public PlayerController player => playerController;
    public PlayerController controller => playerController;
    private Animator animator;
    #endregion

    private bool firingPressed = false;

    public readonly int isAttackingHash = Animator.StringToHash("isAttacking");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");
    public readonly int weaponTypeHash = Animator.StringToHash("weaponType");
    public Dictionary<string, int> bulletsInClips = new Dictionary<string, int>();

    void Start()
    {
        //spawnedWeapon = Instantiate(weaponToSpawn, weaponSocket.transform.position, weaponSocket.transform.rotation, weaponSocket.transform).GetComponent<WeaponComponent>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        //EquipWeapon(spawnedWeapon);
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
        if (!equippedWeapon) return;

        if (playerController.isFiring)
        {
            StopFiring();
        }
        ItemScriptable ammoItem = equippedWeapon.weaponHolder.player.inventory.FindItem(equippedWeapon.stats.weaponName + " Ammo");
        if (!ammoItem || ammoItem.amount <= 0)
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
        if (!equippedWeapon) return;

        if (equippedWeapon.stats.clipSize > 0 && bulletsInClips[equippedWeapon.stats.weaponName] <= 0)
        {
            StartReloading();
            return;
        }

        playerController.isFiring = true;
        animator.SetBool(isAttackingHash, playerController.isFiring);
        equippedWeapon.StartFiring();
    }

    private void StopFiring()
    {
        playerController.isFiring = false;

        if (!equippedWeapon) return;
        equippedWeapon.StopFiring();
    }

    public void FinishFiring()
    {
        animator.SetBool(isAttackingHash, playerController.isFiring);
    }

    public void EquipWeapon(WeaponComponent weaponComponent)
    {
        if (!weaponComponent) return;

        equippedWeapon = weaponComponent;
        equippedWeapon.weaponHolder = this;
        animator.SetInteger(weaponTypeHash, (int)equippedWeapon.stats.type);

        if (!bulletsInClips.ContainsKey(equippedWeapon.stats.weaponName))
            bulletsInClips.Add(equippedWeapon.stats.weaponName, equippedWeapon.stats.clipSize);
        gripIKSocket = equippedWeapon.gripLocation;
        PlayerEvents.InvokeOnWeaponEquipped(weaponComponent);
    }

    public void EquipWeapon(WeaponScriptable weaponScriptable)
    {
        if (!weaponScriptable) return;

        WeaponComponent spawnedWeapon = Instantiate(weaponScriptable.itemPrefab, weaponSocket.transform.position, weaponSocket.transform.rotation, weaponSocket.transform).GetComponent<WeaponComponent>();
        spawnedWeapon.scriptable = weaponScriptable;
        weaponScriptable.Equipped = true;
        if (!spawnedWeapon) return;

        EquipWeapon(spawnedWeapon);
    }

    public void UnequipWeapon()
    {
        if (!equippedWeapon) return;

        equippedWeapon.scriptable.Equipped = false;
        Destroy(equippedWeapon.gameObject);
        equippedWeapon = null;
        animator.SetInteger(weaponTypeHash, 0);
    }
}
