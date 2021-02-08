using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Text variables
    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;
    [SerializeField] private TextMeshProUGUI topScoreText;
    [SerializeField] private TextMeshProUGUI bottomScoreText;

    [SerializeField] private FadeableUI menuUI;
    [SerializeField] private FadeableUI gameOverUI;

    [SerializeField] private TextMeshProUGUI winnerText;

    // Sets menus on the first frame
    private void Start()
    {
        menuUI.FadeIn(true);
        gameOverUI.FadeOut(true);
    }

    // Updates Score Text
    public void UpdateScoreText(int leftScore, int rightScore, int topScore, int bottomScore)
    {
        leftScoreText.text = "Left Paddle: " + leftScore.ToString();
        rightScoreText.text = "Right Paddle: " + rightScore.ToString();
        topScoreText.text = "Top Paddle: " + topScore.ToString();
        bottomScoreText.text = "Bottom Paddle: " + bottomScore.ToString();
    }

    // Main Menu
    public void ShowMenu()
    {
        gameOverUI.FadeOut(false);
        menuUI.FadeIn(false);
    }

    // When the game starts, fades menus
    public void OnGameStart()
    {
        menuUI.FadeOut(false);
        gameOverUI.FadeOut(false);
    }

    // When the game ends it shows who wins
    public void ShowGameOver(int leftScore, int rightScore, int topScore, int bottomScore)
    {
        gameOverUI.FadeIn(false);
        
        if(leftScore == 5)
        {
            winnerText.text = "Player 1";
        }
        if (rightScore == 5)
        {
            winnerText.text = "Player 2";
        }
        if (topScore == 5)
        {
            winnerText.text = "Player 3";
        }
        if (bottomScore == 5)
        {
            winnerText.text = "Player 4";
        }
    }
}
