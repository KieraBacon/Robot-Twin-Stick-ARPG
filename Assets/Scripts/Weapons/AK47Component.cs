using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47Component : WeaponComponent
{
    [SerializeField]
    private LineRenderer aimLine;

    protected override void Fire()
    {
        if (weaponHolder.bulletsInClips[stats.weaponName] > 0 && !isReloading && (stats.fireWhileMoving || !weaponHolder.controller.isRunning))
        {
            --weaponHolder.bulletsInClips[stats.weaponName];
            base.Fire();

            if (firingEffect)
                firingEffect.Play();

            //Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
            Ray weaponRay = new Ray(muzzleLocation.position, weaponHolder.transform.forward);
            if (Physics.Raycast(weaponRay, out RaycastHit hit, stats.range, stats.weaponHitLayers))
            {
                Vector3 hitLocation = hit.point;
                Vector3 hitDirection = hit.point - weaponHolder.transform.position;
                Debug.DrawRay(weaponRay.origin, hitDirection * stats.range, Color.red, 5.0f);
            }
        }
        else if (weaponHolder.bulletsInClips[stats.weaponName] <= 0)
        {
            weaponHolder.StartReloading();
        }
        else
        {
            StopFiring();
        }
    }

    protected override void DrawAimTelegraph()
    {
        Ray weaponRay = new Ray(muzzleLocation.position, weaponHolder.transform.forward);
        
        Vector3 hitLocation = weaponRay.GetPoint(stats.range);
        
        if (Physics.Raycast(weaponRay, out RaycastHit hit, stats.range, stats.weaponHitLayers))
        {
            Debug.Log("Hit!");
            hitLocation = hit.point;
        }

        aimLine.SetPosition(0, muzzleLocation.position);
        aimLine.SetPosition(1, hitLocation);
    }
}
