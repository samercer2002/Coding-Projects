using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // Variables for a scriptable item object
    public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    // Method to use an item
    public virtual void Use()
    {
        // Use the item
        // Something might happen

        Debug.Log("Using " + name);
    }
}
