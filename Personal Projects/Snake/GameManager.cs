using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Bruh
{
    public class GameManager : MonoBehaviour
    {
        // height and length for the map
        public int maxHeight = 15;
        public int maxLength = 17;

        // Colors for the map, player and goal
        public Color color1;
        public Color color2;
        public Color appleColor = Color.red;
        public Color playerColor = Color.green;

        // Camera positioner
        public Transform cameraHolder;

        // Player & goal
        GameObject playerObj;
        GameObject appleObj;
        GameObject tailParent;
        Sprite playerSprite;
        Node playerNode;
        Node appleNode;

        // Map and Renderer
        GameObject mapObject;
        SpriteRenderer mapRenderer;

        Node[,] grid;
        List<Node> availableNodes = new List<Node>();
        List<SpecialNode> tail = new List<SpecialNode>();

        // Direction bools
        bool up, down, left, right;

        public bool isGameOver;
        public bool isFirstInput;

        // Movement & direction operators
        public float moveRate = 0.5f;
        float timer;

        Direction targetDirection;
        Direction currentDirection;

        public Text currentScoreText;
        public Text highScoreText;

        public enum Direction
        {
            up, down, left, right
        }

        int currentScore;
        int highScore;

        public UnityEvent onStart;
        public UnityEvent onGameOver;
        public UnityEvent firstInput;
        public UnityEvent onScore;

        #region Init

        // Calls functions at Start and sets beginning direction
        private void Start()
        {
            onStart.Invoke();
        }

        // Method for a new game
        public void StartNewGame()
        {
            ClearReferences();
            CreateMap();
            PlacePlayer();
            PlaceCamera();
            //targetDirection = Direction.right;
            CreateApple();
            isGameOver = false;
            currentScore = 0;
            UpdateScore();
        }

        // Clears references for the next game's setup
        public void ClearReferences()
        {
            if (mapObject != null)
            {
                Destroy(mapObject);
            }

            if (playerObj != null)
            {
                Destroy(playerObj);
            }
            if (appleObj != null)
            {
                Destroy(appleObj);
            }
            foreach (var t in tail)
            {
                Destroy(t.obj);
            }
            tail.Clear();
            availableNodes.Clear();
            grid = null;
        }

        // Map Creation Method for colors and physical map
        void CreateMap()
        {

            mapObject = new GameObject("Map");
            mapRenderer = mapObject.AddComponent<SpriteRenderer>();

            grid = new Node[maxLength, maxHeight];

            Texture2D txt = new Texture2D(maxLength, maxHeight);
            for (int x = 0; x < maxLength; x++)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    Vector3 tp = Vector3.zero;
                    tp.x = x;
                    tp.y = y;

                    Node n = new Node()
                    {
                        x = x,
                        y = y,
                        worldPosition = tp
                    };

                    grid[x, y] = n;

                    availableNodes.Add(n);

                    #region Visual
                    if (x % 2 != 0)
                    {
                        if (y % 2 != 0)
                        {
                            txt.SetPixel(x, y, color1);
                        }
                        else
                        {
                            txt.SetPixel(x, y, color2);
                        }
                    }
                    else
                    {
                        if (y % 2 != 0)
                        {
                            txt.SetPixel(x, y, color2);
                        }
                        else
                        {
                            txt.SetPixel(x, y, color1);
                        }
                    }

                    #endregion
                }

            }

            // Sets textures for map
            txt.filterMode = FilterMode.Point;

            txt.Apply();
            Rect rect = new Rect(0, 0, maxLength, maxHeight);
            Sprite sprite = Sprite.Create(txt, rect, Vector2.zero, 1, 0, SpriteMeshType.FullRect);
            mapRenderer.sprite = sprite;

        }

        // Places Player
        void PlacePlayer()
        {
            playerObj = new GameObject("Player");
            SpriteRenderer playerRenderer = playerObj.AddComponent<SpriteRenderer>();
            playerSprite = CreateSprite(playerColor);
            playerRenderer.sprite = playerSprite;
            playerRenderer.sortingOrder = 1;
            playerNode = GetNode(3, 3);
            PlacePlayerObject(playerObj, playerNode.worldPosition);
            playerObj.transform.localScale = Vector3.one;

            tailParent = new GameObject("tailParent");
        }

        // Sets Camera
        void PlaceCamera()
        {
            Node n = GetNode(maxLength / 2, maxHeight / 2);
            Vector3 p = n.worldPosition;
            p += Vector3.one * .5f;

            cameraHolder.position = n.worldPosition;
        }

        // Creates Apple
        void CreateApple()
        {
            appleObj = new GameObject("Apple");
            SpriteRenderer appleRenderer = appleObj.AddComponent<SpriteRenderer>();
            appleRenderer.sprite = CreateSprite(appleColor);
            appleRenderer.sortingOrder = 1;
            RandomlyPlaceApple();
        }
        #endregion
        // Updates for input and direction for the player's movement
        #region Update
        private void Update()
        {
            if (isGameOver)
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    onStart.Invoke();
                }
                return;
            }

            GetInput();
            SetPlayerDirection();

            if (isFirstInput)
            {
                timer += Time.deltaTime;
                if (timer > moveRate)
                {
                    timer = 0;
                    currentDirection = targetDirection;
                    MovePlayer();
                }
            }
            else
            {
                if(up || down || left || right)
                {
                    isFirstInput = true;
                    firstInput.Invoke();
                }
            }
        }

        void GetInput()
        {
            up = Input.GetButtonDown("Up");
            down = Input.GetButtonDown("Down");
            left = Input.GetButtonDown("Left");
            right = Input.GetButtonDown("Right");
        }

        void SetPlayerDirection()
        {
            if (up)
            {
                SetDirection(Direction.up);
            }

            else if (down)
            {
                SetDirection(Direction.down);
            }

            else if (left)
            {
                SetDirection(Direction.left);
            }

            else if (right)
            {
                SetDirection(Direction.right);
            }
        }

        void SetDirection(Direction d)
        {
            if(!isOpposite(d))
            {
                targetDirection = d;
                timer = moveRate + 1;
            }
        }

        void MovePlayer()
        {
            int x = 0;
            int y = 0;

            switch(currentDirection)
            {
                case Direction.up:
                    y = 1;
                    break;
                case Direction.down:
                    y = -1;
                    break;
                case Direction.left:
                    x = -1;
                    break;
                case Direction.right:
                    x = 1;
                    break;
            }

            Node targetNode = GetNode(playerNode.x + x, playerNode.y + y);

            if(targetNode == null)
            {
                //Game Over
                onGameOver.Invoke();
            }
            else
            {
                if(isTailNode(targetNode))
                {
                    //Game Over
                    onGameOver.Invoke();
                }
                else
                {

                    bool isScore = false;

                    if (targetNode == appleNode)
                    {
                        // You've Scored
                        isScore = true;

                    }

                    Node previousNode = playerNode;
                    availableNodes.Add(previousNode);

                    if (isScore)
                    {
                        tail.Add(CreateTailNode(previousNode.x, previousNode.y));
                        availableNodes.Remove(previousNode);

                    }

                    MoveTail();

                    PlacePlayerObject(playerObj, targetNode.worldPosition);
                    playerNode = targetNode;
                    availableNodes.Remove(playerNode);

                    if (isScore)
                    {
                        currentScore++;
                        if(currentScore >= highScore)
                        {
                            highScore = currentScore;
                        }

                        onScore.Invoke();

                        if (availableNodes.Count > 0)
                        {
                            RandomlyPlaceApple();
                        }
                        else
                        {
                            // You won
                        }

                    }
                }

            }
        }
        
        // Moves the tail
        void MoveTail()
        {
            Node prevNode = null;

            for(int i = 0; i < tail.Count; i++)
            {
                SpecialNode p = tail[i];
                availableNodes.Add(p.node);

                if(i == 0)
                {
                    prevNode = p.node;
                    p.node = playerNode;
                }
                else
                {
                    Node prev = p.node;
                    p.node = prevNode;
                    prevNode = prev;
                }

                availableNodes.Remove(p.node);
                PlacePlayerObject(p.obj, p.node.worldPosition);
            }
        }
        #endregion
        #region Util

        // Method for the game being over
        public void GameOver()
        {
            isGameOver = true;
            isFirstInput = false;
        }

        // Score method
        public void UpdateScore()
        {
            currentScoreText.text = currentScore.ToString();
            highScoreText.text = highScore.ToString();
        }

        // Denies going backwards over self
        bool isOpposite(Direction d)
        {
            switch(d)
            {
                default:
                case Direction.up:
                    if(currentDirection == Direction.down)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Direction.down:
                    if (currentDirection == Direction.up)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Direction.left:
                    if (currentDirection == Direction.right)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Direction.right:
                    if (currentDirection == Direction.left)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        // Denies going over tail
        bool isTailNode(Node n)
        {
            for(int i = 0; i < tail.Count; i++)
            {
                if(tail[i].node == n)
                {
                    return true;
                }
            }

            return false;
        }

        // Player object position
        void PlacePlayerObject(GameObject obj, Vector3 pos)
        {
            pos += Vector3.zero;
            obj.transform.position = pos;
        }

        // Creates apples in random spots
        void RandomlyPlaceApple()
        {
            int ran = Random.Range(0, availableNodes.Count);
            Node n = availableNodes[ran];
            PlacePlayerObject(appleObj, n.worldPosition);
            appleNode = n;
        }

        // Sets boundaries
        Node GetNode(int x, int y)
        {
            if( x < 0 || x > maxLength - 1 || y < 0 || y > maxHeight - 1)
            {
                return null;
            }

            return grid[x, y];
        }

        // Creates tail sprites
        SpecialNode CreateTailNode(int x, int y)
        {
            SpecialNode s = new SpecialNode();
            s.node = GetNode(x, y);
            s.obj = new GameObject();
            s.obj.transform.parent = tailParent.transform;
            s.obj.transform.position = s.node.worldPosition;
            s.obj.transform.localScale = Vector3.one;
            SpriteRenderer r = s.obj.AddComponent<SpriteRenderer>();
            r.sprite = playerSprite;
            r.sortingOrder = 1;

            return s;
        }

        // Sets sprite position for apple
        Sprite CreateSprite(Color targetColor)
        {
            Texture2D txt = new Texture2D(1, 1);
            txt.SetPixel(0, 0, targetColor);
            txt.Apply();
            txt.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);
            return Sprite.Create(txt, rect, Vector2.zero, 1, 0, SpriteMeshType.FullRect);
        }

        #endregion


    }
}

