using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVolume : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public Vector3 GetPositionInBounds()
    {
        Bounds bounds = boxCollider.bounds;
        return new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
    }
}
