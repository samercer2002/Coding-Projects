using UnityEngine;

public class ItemPickup : Interactable
{
    // Calls for an item object
    public Item item;

    // Method for interacting and picking up an item
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    // Picks up the item and adds it to inventory and destroys the item outside the inventory
    void PickUp()
    {
        Debug.Log("Picking up "+ item.name);

        bool wasPickedUp = Inventory.instance.Add(item);

        if(wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
