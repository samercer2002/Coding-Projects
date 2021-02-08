using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    // Reference variables when applying components
    NavMeshAgent agent;
    Animator animator;

    // constant float for setting the float for the animator
    const float locomotionAnimationSmoothTime = .1f;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the components for the variables
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Sets the speedPercent and applies it to the animator
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
    }
}
