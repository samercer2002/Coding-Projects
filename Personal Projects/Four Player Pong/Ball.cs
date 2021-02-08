using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    // Variables for velocity, movement speed, and the rigidbody of the ball
    private Rigidbody2D rb;
    public Vector2 velocity { get; private set; }
    [SerializeField] private float moveSpeed = 12f;

    // Max angle for bouncing calculations
    [SerializeField] private float maxAngle = 45.0f;

    [SerializeField] private float serveAngle = 45f;

    private bool overridePosition;
    [SerializeField] private float resetTime;

    // Sets variables for the rigidbody and velocity
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector2.left * moveSpeed;
    }
    
    // Sets the rigidbody velocity to the general velocity
    private void FixedUpdate()
    {
        if(!overridePosition)
        {
            rb.velocity = velocity;
        }

        // If ball gets stuck, hit escape
        if(Input.GetKey(KeyCode.Escape))
        {
            Reset(paddle.Side.Left);
        }
    }

    // Set up for paddle collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "vertical")
        {
            PaddleXBounce(collision.collider);

        }
        else if (collision.collider.tag == "horizontal")
        {
            PaddleYBounce(collision.collider);
        }
    }

    // Serving for the ball and sets the direction
    private void Serve(paddle.Side side)
    {
        Vector2 serveDirection = new Vector2(Mathf.Cos(serveAngle * Mathf.Deg2Rad), Mathf.Sin(serveAngle * Mathf.Deg2Rad));
        if(side == paddle.Side.Left)
        {
            serveDirection.x = -serveDirection.x;
        }
        else if(side == paddle.Side.Bottom)
        {
            serveDirection.y = -serveDirection.y;
        }

        velocity = serveDirection * moveSpeed;
    }

    // Bounce for left and right paddles
    private void PaddleXBounce(Collider2D collider)
    {
        float colliderYExtent = collider.bounds.extents.y;
        float yOffset = transform.position.y - collider.transform.position.y;

        float yRatio = yOffset / colliderYExtent;

        float bounceAngle = maxAngle * yRatio * Mathf.Deg2Rad;

        Vector2 bounceDirection = new Vector2(Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle));
        bounceDirection.x *= Mathf.Sign(-velocity.x);

        velocity = bounceDirection * moveSpeed;
    }
    
    // Bounce for top and bottom paddles
    private void PaddleYBounce(Collider2D collider)
    {
        float colliderXExtent = collider.bounds.extents.x;
        float xOffset = transform.position.x - collider.transform.position.x;

        float xRatio = xOffset / colliderXExtent;

        float bounceAngle = maxAngle * xRatio * Mathf.Deg2Rad;

        Vector2 bounceDirection = new Vector2(Mathf.Sin(bounceAngle), Mathf.Cos(bounceAngle));
        bounceDirection.y *= Mathf.Sign(-velocity.y);

        velocity = bounceDirection * moveSpeed;
    }

    // Used to reset the ball's position when scoring
    public void Reset(paddle.Side side)
    {
        StartCoroutine(ResetRoutine(side));
    }

    private IEnumerator ResetRoutine(paddle.Side side)
    {
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;

        overridePosition = true;

        yield return new WaitForSeconds(resetTime);
        overridePosition = false;
        Serve(side);
    }
}
