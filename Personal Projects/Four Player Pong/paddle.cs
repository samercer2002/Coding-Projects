using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paddle : MonoBehaviour
{
    // Determines speed of the paddles
    [SerializeField] public float moveSpeed = 5f;

    // Determines AI type
    public bool isAIVert;
    public bool isAIHorz;

    // Variables for ball and a box collider
    private Ball ball;
    private BoxCollider2D box;

    // Offsets for paddles
    private float randomYOffset;
    private float randomXOffset;

    // Direction and determining when the ball is incoming
    private Vector2 forwardDirection;
    private bool firstIncoming;

    // List of sides for paddles
    public enum Side { Left, Right, Top, Bottom }

    [SerializeField] private Side side;

    // Variables for resetting the position of the paddles
    [SerializeField] private float resetTime;
    private bool overridePosition;

    // Start function
    private void Start()
    {
        // Setting the variables of ball and box
        ball = GameObject.FindGameObjectWithTag("ball").GetComponent<Ball>();
        box = GetComponent<BoxCollider2D>();

        // Setting up forward directions for detecting whether the ball is incoming
        if (side == Side.Left)
        {
            forwardDirection = Vector2.right;
        }
        else if (side == Side.Right)
        {
            forwardDirection = Vector2.left;
        }
        else if (side == Side.Top)
        {
            forwardDirection = Vector2.down;
        }
        else if (side == Side.Bottom)
        {
            forwardDirection = Vector2.up;
        }
    }

    // Updates the position of the paddles
    private void Update()
    {
        if(!overridePosition)
        {
            MovePaddle();
        }
    }

    // Sets limits for the paddles and references movement
    private void MovePaddle()
    {
        float targetYPosition = GetNewYPosition();
        float targetXPosition = GetNewXPosition();

        ClampPosition(ref targetYPosition, ref targetXPosition);
        if (isAIVert)
        {
            transform.position = new Vector3(transform.position.x, targetYPosition, transform.position.z);
        }
        else if (isAIHorz)
        {
            transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(targetXPosition, targetYPosition, transform.position.z);
        }
    }

    // Sets max and min positions for paddles using the camera
    private void ClampPosition(ref float yPosition, ref float xPosition)
    {
        float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y;
        float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;
        float minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x;
        float maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;

        yPosition = Mathf.Clamp(yPosition, minY, maxY);
        xPosition = Mathf.Clamp(xPosition, minX, maxX);
    }

    // Sets the Y position of the Left and Right paddles
    private float GetNewYPosition()
    {
        float result = transform.position.y;
        // AI movement
        if(isAIVert)
        {
            if(Incoming())
            {

                if(firstIncoming)
                {
                    firstIncoming = false;
                    randomYOffset = GetRandomYOffset();
                }
                result = Mathf.MoveTowards(transform.position.y, ball.transform.position.y + randomYOffset, moveSpeed * Time.deltaTime);
            }
            else
            {
                firstIncoming = true;
            }
        }
        else if(isAIHorz)
        {
            return 0;
        }
        // Player movement
        else if(side == Side.Left)
        {
            float movement = Input.GetAxisRaw("Vertical " + side.ToString()) * moveSpeed * Time.deltaTime;
            result = transform.position.y + movement;
        }
        else if(side == Side.Right)
        {
            float movement = Input.GetAxisRaw("Vertical " + side.ToString()) * moveSpeed * Time.deltaTime;
            result = transform.position.y + movement;
        }
        return result;
    }

    // Sets X position for top and bottom paddles
    private float GetNewXPosition()
    {
        float result = transform.position.x;
        // AI movement
        if (isAIHorz)
        {
            if(Incoming())
            {
                if (firstIncoming)
                {
                    firstIncoming = false;
                    randomXOffset = GetRandomXOffset();
                }

                result = Mathf.MoveTowards(transform.position.x, ball.transform.position.x + randomXOffset, moveSpeed * Time.deltaTime);
            }
            else
            {
                firstIncoming = true;
            }
        }
        // Player movement
        else if(side == Side.Top)
        {
            float movement = Input.GetAxisRaw("Horizontal " + side.ToString()) * moveSpeed * Time.deltaTime;
            result = transform.position.x + movement;
            //Debug.Log("ButtonPress");
        }
        else if(side == Side.Bottom)
        {
            float movement = Input.GetAxisRaw("Horizontal " + side.ToString()) * moveSpeed * Time.deltaTime;
            result = transform.position.x + movement;
        }
        return result;
    }

    // Determines if ball is incoming
    private bool Incoming()
    {
        float dotProduct = Vector2.Dot(ball.velocity, forwardDirection);
        return dotProduct < 0f;
    }

    // Random Offset for left and right paddles
    private float GetRandomYOffset()
    {
        float maxOffset = box.bounds.extents.y;
        return Random.Range(-maxOffset, maxOffset);

    }

    // Random Offset for top and bottom paddles
    private float GetRandomXOffset()
    {
        float maxOffset = box.bounds.extents.x;
        return Random.Range(-maxOffset, maxOffset);
    }


    // Used to reset the paddles when a score occurs
    public void ResetY()
    {
        StartCoroutine(ResetYRoutine());
    }
    public void ResetX()
    {
        StartCoroutine(ResetXRoutine());
    }

    // Routine for the left and right paddle
    private IEnumerator ResetYRoutine()
    {
        overridePosition = true;
        float startPosition = transform.position.y;
        for(float timer = 0; timer < resetTime; timer += Time.deltaTime)
        {
            float targetPosition = Mathf.Lerp(startPosition, 0f, timer / resetTime);
            transform.position = new Vector3(transform.position.x, targetPosition, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        overridePosition = false;
    }

    // Routine for the top and bottom paddle
    private IEnumerator ResetXRoutine()
    {
        overridePosition = true;
        float startPosition = transform.position.x;
        for (float timer = 0; timer < resetTime; timer += Time.deltaTime)
        {
            float targetPosition = Mathf.Lerp(startPosition, 0, timer / resetTime);
            transform.position = new Vector3(targetPosition, transform.position.y, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        overridePosition = false;
    }
}
