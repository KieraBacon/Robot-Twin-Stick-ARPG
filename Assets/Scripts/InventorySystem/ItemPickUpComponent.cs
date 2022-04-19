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

    [SerializeField] private GameObject propMesh;

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
        propMesh.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void ApplyMesh()
    {
        propMesh = Instantiate(pickupItem.itemPrefab);
        propMesh.transform.position = transform.position;
     
        Destroy(propMesh.GetComponent<WeaponComponent>());
        foreach (Collider collider in propMesh.GetComponentsInChildren<Collider>())
            Destroy(collider);
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

        Destroy(propMesh.gameObject);
        Destroy(gameObject);
    }
}
