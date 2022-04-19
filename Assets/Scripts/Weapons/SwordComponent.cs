using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordComponent : WeaponComponent
{
    [SerializeField]
    private LinkedList<Vector3> hitLocations = new LinkedList<Vector3>();
    [SerializeField]
    private float hitAngle;
    protected override void Fire()
    {
        hitLocations.Clear();
        if ((stats.fireWhileMoving || !weaponHolder.controller.isRunning))
        {
            base.Fire();

            Collider[] hitColliders = Physics.OverlapSphere(weaponHolder.transform.position, stats.range, stats.weaponHitLayers);
            foreach (Collider collider in hitColliders)
            {
                float angle = Vector3.SignedAngle(weaponHolder.transform.forward, collider.transform.position - weaponHolder.transform.position, Vector3.up);
                Debug.Log("angle: " + angle);
                if (Mathf.Abs(angle) > hitAngle) continue;

                hitLocations.AddLast(collider.transform.position);
                DealDamage(collider.gameObject);
                //Debug.DrawRay(weaponHolder.transform.position, collider.transform.position - weaponHolder.transform.position, Color.green, 2);
            }


            //RaycastHit[] hits = Physics.SphereCastAll(weaponHolder.transform.position, stats.range, weaponHolder.transform.forward, 0.1f, stats.weaponHitLayers);
            //foreach (RaycastHit hit in hits)
            //{
            //    if (hit.collider.gameObject == weaponHolder.gameObject) continue;
            //
            //    float angle = Vector3.SignedAngle(weaponHolder.transform.forward, hit.point - weaponHolder.transform.position, Vector3.up);
            //    Debug.DrawLine(weaponHolder.transform.position, hit.point, Color.magenta, 2);
            //    Debug.DrawRay(Vector3.zero, hit.point - weaponHolder.transform.position, Color.green, 2);
            //    Debug.Log("angle: " + angle + " name: " + hit.collider.gameObject.name);
            //    if (angle > hitAngle) return;
            //
            //    hitLocations.AddLast(hit.point);
            //    DealDamage(hit);
            //}
        }
    }


    void DealDamage(GameObject hitInfo)
    {
        Debug.Log(Time.time + " Layer: " + LayerMask.LayerToName(hitInfo.gameObject.layer) + " Name: " + hitInfo.gameObject.name);
        hitInfo.GetComponent<IDamageable>()?.TakeDamage(weaponHolder.gameObject, stats.damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 hitLocation in hitLocations)
        {
            Gizmos.DrawWireSphere(hitLocation, 0.1f);
        }
    }

    protected override void DrawAimTelegraph()
    {
    }
}

