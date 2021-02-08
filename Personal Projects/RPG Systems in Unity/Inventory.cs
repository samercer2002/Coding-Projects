using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    // Instantiates the inventory instance
    public static Inventory instance;

    // Called before the start of the game
    // Makes sure there is only one instance of an inventory
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion
    // Determines if items have changed
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    // Inventory max space
    public int space = 20;
    
    // List of item objects
    public List<Item> items = new List<Item>();

    // Determines if the item can be added to the inventory and adds it if true
    public bool Add(Item item)
    {
        if(!item.isDefaultItem)
        {
            if(items.Count >= space)
            {
                Debug.Log("Not enough room.");
                return false;
            }
            items.Add(item);

            if(onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }

        return true;
    }

    // Method for removing the items from the inventory
    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
