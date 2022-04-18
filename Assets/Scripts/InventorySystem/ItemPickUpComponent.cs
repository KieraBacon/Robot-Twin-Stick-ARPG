using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpComponent : MonoBehaviour
{
    [SerializeField]
    private ItemScriptable pickupItem;

    [Tooltip("Manual override for the drop amount, if left at -1, it will use the amount from the scriptable object.")]
    [SerializeField] private int amount = -1;
    [SerializeField] private float rotationSpeed = 10.0f;

    [SerializeField] private MeshRenderer propMeshRenderer;
    [SerializeField] private MeshFilter propMeshFilter;

    private ItemScriptable itemInstance;

    private void Start()
    {
        InstantiateItem();
    }

    private void InstantiateItem()
    {
        itemInstance = Instantiate(pickupItem);
        if (amount > 0)
        {
            itemInstance.SetAmount(amount);
        }
        ApplyMesh();
    }

    private void Update()
    {
        propMeshFilter.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void ApplyMesh()
    {
        MeshFilter prefabMeshFilter = pickupItem.itemPrefab.GetComponentInChildren<MeshFilter>();
        if (propMeshFilter) propMeshFilter.mesh = prefabMeshFilter.sharedMesh;
        if (propMeshRenderer) propMeshRenderer.materials = prefabMeshFilter.GetComponent<MeshRenderer>().sharedMaterials;
        propMeshRenderer.transform.rotation = prefabMeshFilter.transform.rotation;
        propMeshRenderer.transform.localScale = prefabMeshFilter.transform.lossyScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        InventoryComponent playerInventory = other.GetComponent<InventoryComponent>();
        if (playerInventory)
        {
            playerInventory.AddItem(itemInstance, amount);
        }
        // Add to intventory here
        // Get reference to the player inventory, add item to it

        Destroy(gameObject);
    }
}
