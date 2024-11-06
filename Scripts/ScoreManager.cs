using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI opponentScoreText;
    public TextMeshProUGUI roundCounterTextP;
    public TextMeshProUGUI roundCounterTextO;
    public TextMeshProUGUI turnText;

    private int playerScore = 0;
    private int opponentScore = 0;
    private int playerRoundCount = 1; // Separate round count for the player & opponent
    private int opponentRoundCount = 0;


    public void UpdatePlayerScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        playerScoreText.text = playerScore.ToString();
    }

    public void UpdateOpponentScore(int scoreToAdd)
    {
        opponentScore += scoreToAdd;
        opponentScoreText.text = opponentScore.ToString();
    }

    public void NextRoundP()
    {
        playerRoundCount++;
        roundCounterTextP.text = playerRoundCount.ToString();
    }

    public void NextRoundO()
    {
        opponentRoundCount++;
        roundCounterTextO.text = opponentRoundCount.ToString();
    }

    public void DisplayPlayerTurn()
    {
        turnText.text = "<color=#79A3E2>Your Turn!</color>";
    }

    public void DisplayOpponentTurn()
    {
        turnText.text = "<color=#FF0000>Opponet Turn!</color>";
    }

    public void DisplayPlayerWin()
    {
        turnText.text = "<color=#79A3E2>You won!</color>";
    }

    public void DisplayOpponentWin()
    {
        turnText.text = "<color=#FF0000>Opponent won!</color>";
    }

    public void ResetScoresAndRounds()
    {
        playerScore = 0;
        opponentScore = 0;
        playerRoundCount = 1;
        opponentRoundCount = 1;
        playerScoreText.text = playerScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
        roundCounterTextP.text = playerRoundCount.ToString();
        roundCounterTextO.text = opponentRoundCount.ToString();
    }
}
