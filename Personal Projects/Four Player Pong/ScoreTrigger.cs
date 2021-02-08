using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    // Sets side that doesn't score from ball entering the triggers
    [SerializeField] private paddle.Side paddleDoesNotScored;

    // Ball entering the trigger
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Ball ball = collider.GetComponent<Ball>();

        if(ball)
        {
            GameController.instance.Score(paddleDoesNotScored);
        }
    }
}
