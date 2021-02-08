using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

// Requires the PlayerMotor script
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    // variable for player's focus
    public Interactable focus;

    // Sets the layer where the player can move
    public LayerMask movementMask;

    // Defines camera to be used for controlling the character
    Camera cam;

    // Cross references the PlayerMotor script for movement
    PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        // Sets which camera to use
        cam = Camera.main;

        // Sets the variable to the component of PlayerMotor on the player
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // If left mouse button is pressed
        if(Input.GetMouseButtonDown(0))
        {
            // Sets a ray to the current cursor position
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            // Where the click hits
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movementMask))
            {
                // Moves player to hit
                motor.MoveToPoint(hit.point);

                // Stop focusing any objects
                RemoveFocus();
            }
        }
        
        // For interaction purposes
        else if(Input.GetMouseButtonDown(1))
        {
            // Sets a ray to the current cursor position
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            // Where the click hits
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                // Check to see if hits interactable
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                // if so, set to focus
                if(interactable != null)
                {
                    // Sets the player to focus on a target
                    SetFocus(interactable);
                }
            }
        }
    }

    // Method for setting focus
    void SetFocus(Interactable newFocus)
    {
        // When there is a new focus
        if(newFocus != focus)
        {
            // if there is a change in focus
            if(focus != null)
            {
                // Defocus on the current target
                focus.OnDefocused();
            }
            
            // Sets the player's focus
            focus = newFocus;
            newFocus.OnFocused(transform);
        }

        // Go to the focus
        motor.FollowTarget(newFocus);
    }

    // Method for removing the focus
    void RemoveFocus()
    {
        // If there is a current focus, defocus
        if(focus != null)
        {
            focus.OnDefocused();
        }
        
        // Stop following the last focus
        focus = null;
        motor.StopFollowingTarget();
    }
}
