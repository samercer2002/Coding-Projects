using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance { get; private set; }

    // Used for UI Manager to feed scores
    [SerializeField] private UIManager uiManager;

    // Score and paddle Scores
    [SerializeField] private int scoreToWin = 10;
    [SerializeField] private int leftScore;
    [SerializeField] private int rightScore;
    [SerializeField] private int topScore;
    [SerializeField] private int bottomScore;

    // If in the main menu
    [SerializeField] private bool inMenu;

    // Paddles
    [SerializeField] private paddle leftPaddle;
    [SerializeField] private paddle rightPaddle;
    [SerializeField] private paddle topPaddle;
    [SerializeField] private paddle bottomPaddle;

    private paddle.Side serveSide;

    private Ball ball;

    // Finds ball for scoring
    private void Awake()
    {
        instance = this;
        ball = GameObject.FindGameObjectWithTag("ball").GetComponent<Ball>();

        DoMenu();
    }

    private void DoMenu()
    {
        inMenu = true;
        leftPaddle.isAIVert = rightPaddle.isAIVert = true;
        topPaddle.isAIHorz = bottomPaddle.isAIHorz = true;
        leftScore = rightScore = topScore = bottomScore = 0;
        uiManager.UpdateScoreText(leftScore, rightScore, topScore, bottomScore);
        ball.gameObject.SetActive(true);
        ResetGame();
    }

    // Calculates score (to be redone)
    public void Score(paddle.Side side)
    {
        if(side == paddle.Side.Left)
        {
            rightScore++;
            topScore++;
            bottomScore++;
        }
        else if(side == paddle.Side.Right)
        {
            leftScore++;
            topScore++;
            bottomScore++;
        }
        else if(side == paddle.Side.Top)
        {
            leftScore++;
            rightScore++;
            bottomScore++;
        }
        else if(side == paddle.Side.Bottom)
        {
            leftScore++;
            rightScore++;
            topScore++;
        }
        uiManager.UpdateScoreText(leftScore, rightScore, topScore, bottomScore);
        serveSide = side;

        if (IsGameOver())
        {
            if(inMenu)
            {
                ResetGame();
                leftScore = topScore = bottomScore = rightScore = 0;
            }
            else
            {
                ball.gameObject.SetActive(false);

                uiManager.ShowGameOver(leftScore, rightScore, topScore, bottomScore);
            }
        }
        else
        {
            ResetGame();
        }
    }

    // Used to determine if the game is over
    private bool IsGameOver()
    {
        bool result = false;

        if(leftScore >= scoreToWin || rightScore >= scoreToWin || topScore >= scoreToWin || bottomScore >= scoreToWin)
        {
            return true;
        }
        return result;
    }

    // Resets ball and paddles
    private void ResetGame()
    {
        ball.gameObject.SetActive(true);
        ball.Reset(serveSide);
        leftPaddle.ResetY();
        rightPaddle.ResetY();
        topPaddle.ResetX();
        bottomPaddle.ResetX();
    }

    // Sets up the game when selecting how many players and quitting
    #region UIMethods
    public void StartOnePlayer()
    {
        leftPaddle.isAIVert = false;
        rightPaddle.isAIVert = true;
        topPaddle.isAIHorz = true;
        bottomPaddle.isAIHorz = true;
        InitializeGame();
    }
    public void StartTwoPlayer()
    {
        leftPaddle.isAIVert = true;
        rightPaddle.isAIVert = false;
        topPaddle.isAIHorz = true;
        bottomPaddle.isAIHorz = true;
        InitializeGame();
    }
    public void StartThreePlayer()
    {
        leftPaddle.isAIVert = false;
        rightPaddle.isAIVert = false;
        topPaddle.isAIHorz = false;
        bottomPaddle.isAIHorz = true;
        InitializeGame();
    }
    public void StartFourPlayer()
    {
        leftPaddle.isAIVert = false;
        rightPaddle.isAIVert = false;
        topPaddle.isAIHorz = false;
        bottomPaddle.isAIHorz = false;
        InitializeGame();
    }

    public void Replay()
    {
        InitializeGame();
        uiManager.OnGameStart();
    }

    public void GoToMenu()
    {
        uiManager.ShowMenu();
        DoMenu();
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void InitializeGame()
    {
        inMenu = false;
        leftScore = rightScore = topScore = bottomScore = 0;
        uiManager.UpdateScoreText(leftScore, rightScore, topScore, bottomScore);
        ResetGame();
        uiManager.OnGameStart();
    }
#endregion
}
