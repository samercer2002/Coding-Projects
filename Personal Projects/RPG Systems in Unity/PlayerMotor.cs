using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

// Requires and places the component NavMeshAgent onto anything using script
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    // Target for player to follow
    Transform target;

    // Declares the navmeshagent component in script
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the varibale to the agent in Unity
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Updates the player's target and how the player will look at it
    void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    // When called, sets the destination to where the point is
    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    // Method for following target and what distance to stop at
    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * .8f;
        agent.updateRotation = false;

        target = newTarget.interactionTransform;
    }

    // Method for the change in targets and to stop following current target
    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;

        target = null;
    }

    // Sets player's direction to look at the target
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
