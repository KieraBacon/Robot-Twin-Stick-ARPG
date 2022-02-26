using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47Component : WeaponComponent
{
    protected override void Fire()
    {
        if (stats.bulletsInClip > 0 && !isReloading && (stats.fireWhileMoving || !weaponHolder.controller.isRunning))
        {
            --stats.bulletsInClip;
            base.Fire();

            if (firingEffect)
                firingEffect.Play();

            Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
            if (Physics.Raycast(screenRay, out RaycastHit hit, stats.range, stats.weaponHitLayers))
            {
                Vector3 hitLocation = hit.point;
                Vector3 hitDirection = hit.point - mainCamera.transform.position;
                Debug.DrawRay(mainCamera.transform.position, hitDirection * stats.range, Color.red, 5.0f);
            }
        }
        else if (stats.bulletsInClip <= 0)
        {
            weaponHolder.StartReloading();
        }
        else
        {
            StopFiring();
        }
    }
}
