using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Radius for interactables
    public float radius = 3f;
    
    // variable for interactable object's transform
    public Transform interactionTransform;

    // bool for if the player is focus
    bool isFocus = false;
    
    // variable for player's transform
    Transform player;

    // bool for if the player has interacted yet
    bool hasInteracted = false;

    // method of what happens when the player is interacting
    public virtual void Interact()
    {
        // This method is meant to be overwriten
        Debug.Log("Interacting with " + transform.name);
    }

    // Update method
    void Update()
    {
        // If the player has a target and hasn't interacted yet
        if(isFocus && !hasInteracted)
        {
            // go and try to interact with the target
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if(distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    // Method when there is a focus
    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    // When defocusing on an object
    public void OnDefocused()
    {
        isFocus = false;
        player = null;
    }

    // For developer visual purposes of seeing the radius of the interactable
    void OnDrawGizmosSelected()
    {
        if(interactionTransform == null)
        {
            interactionTransform = transform;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
