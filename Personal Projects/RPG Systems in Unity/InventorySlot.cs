using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Variables for choosing an icon and button in the Unity heirarchy
    public Image icon;
    public Button removeButton;

    // Item object instantiation
    Item item;

    // Adds the item to the inventory slot and applies the sprite
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    // Clears the slot when the button is pressed
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    // Removes the button when the inventory slot is empty
    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    // Uses the item
    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }

}
