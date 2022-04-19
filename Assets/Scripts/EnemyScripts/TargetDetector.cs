using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    public delegate void TargetEventHandler(GameObject target);
    public event TargetEventHandler onNewTargetInRange;
    HashSet<GameObject> targetsInRange = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetsInRange.Add(other.gameObject);
            onNewTargetInRange?.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetsInRange.Remove(other.gameObject);
        }
    }

    public GameObject GetNearestTarget()
    {
        GameObject selectedTarget = null;
        float selectedTargetDistance = float.PositiveInfinity;

        foreach (GameObject target in targetsInRange)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (!selectedTarget || distance < selectedTargetDistance)
            {
                selectedTarget = target;
                selectedTargetDistance = distance;
            }
        }

        return selectedTarget;
    }
}
