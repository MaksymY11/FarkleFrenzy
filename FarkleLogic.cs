using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarkleLogic : MonoBehaviour
{
    [SerializeField] private List<DieRoller2D> playerDice; // Dice for the player
    [SerializeField] private List<DieRoller2D> opponentDice; // Dice for the opponent
    [SerializeField] private int playerTotalScore;
    [SerializeField] private int opponentTotalScore;
    [SerializeField] private int tempScore;
    [SerializeField] private int WinningScore = 5000;
    [SerializeField] private ScoreManager scoreManager; // Reference to ScoreManager
    [SerializeField] private FarkleUI farkleSound;
    [SerializeField] private WinUI winSound;
    [SerializeField] private LoseUI looseSound;

    private bool playerTurn = true; // Indicates if it's the player's turn
    private bool isFarkle = false; // Indicates if a Farkle has been rolled

    void Start()
    {
        DeactivateDice(opponentDice);
    }

    public void StartTurn(bool isPlayerTurn)
    {
        tempScore = 0;
        isFarkle = false;

        if (isPlayerTurn)
        {
            playerTurn = true;
            Debug.Log("Starting Player's turn");
            scoreManager.DisplayPlayerTurn();
            scoreManager.NextRoundP();
            ActivateDice(playerDice);
            DeactivateDice(opponentDice); // Deactivate opponent's dice at the start of the player's turn
        }
        else
        {
            playerTurn = false;
            Debug.Log("Starting Opponent's turn");
            scoreManager.DisplayOpponentTurn();
            scoreManager.NextRoundO();
            ActivateDice(opponentDice);
            DeactivateDice(playerDice); // Deactivate player's dice at the start of the opponent's turn
            StartCoroutine(OpponentTakeTurn());
        }
    }

    private void ActivateDice(List<DieRoller2D> diceList)
    {
        foreach (var d in diceList)
        {
            d.ToggleActive(true);
            d.ResetSelection();
        }
    }

    private void DeactivateDice(List<DieRoller2D> diceList)
    {
        foreach (var d in diceList)
        {
            d.ToggleActive(false);
        }
    }

    void Update()
    {
        if (playerTurn) // Ensure input only affects the player's turn
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                BankScore();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !isFarkle)
            {
                RollRemainingDice(playerDice); // Only roll player dice
                StartCoroutine(CheckForFarkleAfterRoll(playerDice)); // Start Farkle check after roll
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                EndTurn();
            }
        }
    }

    private void RollRemainingDice(List<DieRoller2D> diceList)
    {
        foreach (var d in diceList)
        {
            if (d.gameObject.activeSelf && !d.IsSelected())
            {
                d.RollDie();
            }
        }
    }




    public void BankScore()
    {
        if (playerTurn)
        {
            // Calculate and bank the player's score
            CalcScore(); // Use the original method for the player
            playerTotalScore += tempScore;
            scoreManager.UpdatePlayerScore(tempScore);
            tempScore = 0;

            foreach (var d in playerDice)
            {
                if (d.IsSelected())
                {
                    d.ResetSelection();
                    d.ToggleActive(false); // Deactivate banked dice for the player
                }
            }

            Debug.Log("Player's score banked.");
            CheckForWinCondition();
        }
        else
        {
            // Calculate and bank the opponent's score
            CalcScoreOP(); // Use the special opponent method
            opponentTotalScore += tempScore;
            scoreManager.UpdateOpponentScore(tempScore);
            tempScore = 0;

            Debug.Log("Opponent's score banked.");
            CheckForWinCondition();
        }
    }

    private void CheckForWinCondition()
    {
        if (playerTotalScore >= WinningScore)
        {
            scoreManager.DisplayPlayerWin();
            winSound.PlayWinSound();
            EndGame();
        }
        else if (opponentTotalScore >= WinningScore)
        {
            scoreManager.DisplayOpponentWin();
            looseSound.PlayLoseSound();
            EndGame();
        }
    }

    private void EndGame()
    {
        Application.Quit();
    }

    public void CalcScore()
    {
        tempScore = 0;
        Dictionary<int, int> diceCount = new Dictionary<int, int>();

        foreach (var d in playerDice)
        {
            if (d.IsSelected())
            {
                int result = d.Result;
                if (!diceCount.ContainsKey(result))
                {
                    diceCount[result] = 0;
                }
                diceCount[result]++;
            }
        }

        Debug.Log("Player's dice counts for scoring: " + string.Join(", ", diceCount.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
        tempScore += CalculateCombinationScore(diceCount);
        Debug.Log($"Player's temp score after calculation: {tempScore}");
    }

    private void CalcScoreOP()
    {
        tempScore = 0;
        Dictionary<int, int> diceCount = new Dictionary<int, int>();

        foreach (var d in opponentDice)
        {
            if (d.gameObject.activeSelf)
            {
                int result = d.Result;
                if (!diceCount.ContainsKey(result))
                {
                    diceCount[result] = 0;
                }
                diceCount[result]++;
            }
        }

        Debug.Log("Opponent's dice counts for scoring: " + string.Join(", ", diceCount.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
        tempScore += CalculateCombinationScoreOP(diceCount, opponentDice);
        Debug.Log($"Opponent's temp score after calculation: {tempScore}");
    }

    private int CalculateCombinationScore(Dictionary<int, int> diceCount)
    {
        int score = 0;
        bool hasStraight = diceCount.Count == 6 && diceCount.Keys.OrderBy(k => k).SequenceEqual(new List<int> { 1, 2, 3, 4, 5, 6 });
        bool hasThreePairs = diceCount.Count == 3 && diceCount.Values.All(v => v == 2);

        foreach (var kvp in diceCount)
        {
            int result = kvp.Key;
            int count = kvp.Value;

            if (count >= 3)
            {
                switch (count)
                {
                    case 3:
                        score += (result == 1) ? 300 : result * 100;
                        break;
                    case 4:
                        score += 1000;
                        break;
                    case 5:
                        score += 2000;
                        break;
                    case 6:
                        score += 3000;
                        break;
                }
            }

            if (count < 3)
            {
                if (result == 1)
                {
                    score += 100 * count;
                }
                else if (result == 5)
                {
                    score += 50 * count;
                }
            }
        }

        if (hasStraight)
        {
            score += 1500;
        }
        else if (hasThreePairs)
        {
            score += 1500;
        }

        return score;
    }

    private int CalculateCombinationScoreOP(Dictionary<int, int> diceCount, List<DieRoller2D> diceList)
    {
        int score = 0;

        // Check for special combinations first (straight and three pairs)
        bool hasStraight = diceCount.Count == 6 && diceCount.Keys.OrderBy(k => k).SequenceEqual(new List<int> { 1, 2, 3, 4, 5, 6 });
        bool hasThreePairs = diceCount.Count == 3 && diceCount.Values.All(v => v == 2);

        if (hasStraight)
        {
            score = 1500;
            Debug.Log("Opponent scored a straight (1-6) for 1500 points.");
            StartCoroutine(DelayedDeactivateAllDice(diceList));
            return score;
        }

        if (hasThreePairs)
        {
            score = 1500;
            Debug.Log("Opponent scored three pairs for 1500 points.");
            StartCoroutine(DelayedDeactivateAllDice(diceList));
            return score;
        }

        foreach (var kvp in diceCount)
        {
            int result = kvp.Key;
            int count = kvp.Value;

            if (count >= 3)
            {
                switch (count)
                {
                    case 3:
                        score = (result == 1) ? 300 : result * 100;
                        break;
                    case 4:
                        score = 1000;
                        break;
                    case 5:
                        score = 2000;
                        break;
                    case 6:
                        score = 3000;
                        break;
                }

                Debug.Log($"Opponent scored {score} with {count} of {result}s.");
                StartCoroutine(DelayedDeactivateDice(diceList, result, count)); // Delayed deactivation for specific dice
                return score;
            }

            if (count < 3)
            {
                if (result == 1)
                {
                    score = 100 * count;
                    Debug.Log($"Opponent scored {score} with {count} single 1s.");
                    StartCoroutine(DelayedDeactivateDice(diceList, result, count));
                    return score;
                }
                else if (result == 5)
                {
                    score = 50 * count;
                    Debug.Log($"Opponent scored {score} with {count} single 5s.");
                    StartCoroutine(DelayedDeactivateDice(diceList, result, count));
                    return score;
                }
            }
        }

        Debug.Log($"Total score for this calculation: {score}");
        return score;
    }

    // Coroutine to deactivate all dice after a delay
    private IEnumerator DelayedDeactivateAllDice(List<DieRoller2D> diceList)
    {
        yield return new WaitForSeconds(2f); // Adjust the delay as needed
        foreach (var d in diceList)
        {
            d.ToggleActive(false);
        }
    }

    // Coroutine to deactivate specific dice after a delay
    private IEnumerator DelayedDeactivateDice(List<DieRoller2D> diceList, int result, int count)
    {
        yield return new WaitForSeconds(2f); // Adjust the delay as needed
        foreach (var d in diceList.Where(d => d.Result == result && d.gameObject.activeSelf).Take(count))
        {
            d.ToggleActive(false);
        }
    }

    private void DeactivateAllDice(List<DieRoller2D> diceList)
    {
        foreach (var d in diceList)
        {
            d.ToggleActive(false);
        }
    }

    private IEnumerator OpponentTakeTurn()
    {
        yield return new WaitForSeconds(1.5f); // Initial delay to simulate thinking before rolling
        RollRemainingDice(opponentDice);

        yield return new WaitUntil(() => AllDiceStoppedRolling(opponentDice)); // Wait until dice stop rolling

        CalcScoreOP(); // Calculate the score for the opponent
        int potentialScore = tempScore;

        if (potentialScore > 0)
        {
            opponentTotalScore += potentialScore;
            scoreManager.UpdateOpponentScore(potentialScore);
            Debug.Log("Opponent scored: " + potentialScore);

            yield return new WaitForSeconds(1.5f); // Delay before deciding to continue or bank

            if (ShouldOpponentContinue())
            {
                Debug.Log("Opponent decides to continue rolling.");
                yield return new WaitForSeconds(1.5f); // Delay before the next roll
                StartCoroutine(OpponentTakeTurn()); // Continue rolling if the opponent decides to
            }
            else
            {
                Debug.Log("Opponent decides to bank the score.");
                yield return new WaitForSeconds(1.5f); // Delay before banking the score
                BankScore(); // Bank the score for the opponent
                yield return new WaitForSeconds(1.5f); // Delay before ending the turn
                EndTurn(); // End the turn after banking
            }
        }
        else
        {
            Debug.Log("Opponent rolled a Farkle!");
            yield return new WaitForSeconds(1.5f); // Delay to show Farkle message before ending turn
            EndTurn(); // End the turn on a Farkle
        }
    }

    private bool ShouldOpponentContinue()
    {
        int riskThreshold = 1500; // Adjust this value for AI difficulty
        return (opponentTotalScore + tempScore < riskThreshold);
    }

    private IEnumerator CheckForFarkleAfterRoll(List<DieRoller2D> diceList)
    {
        yield return new WaitUntil(() => AllDiceStoppedRolling(diceList));

        Dictionary<int, int> activeDiceCount = new Dictionary<int, int>();

        foreach (var d in diceList)
        {
            if (d.gameObject.activeSelf)
            {
                int result = d.Result;
                if (!activeDiceCount.ContainsKey(result))
                {
                    activeDiceCount[result] = 0;
                }
                activeDiceCount[result]++;
            }
        }

        Debug.Log("Active dice counts: " + string.Join(", ", activeDiceCount.Select(kvp => $"{kvp.Key}: {kvp.Value}")));

        int potentialScore = CalculateCombinationScore(activeDiceCount);
        Debug.Log($"Calculated potential score: {potentialScore}");

        if (potentialScore > 0)
        {
            isFarkle = false;
        }
        else
        {
            isFarkle = true;
            farkleSound.ShowFarklePopUp();
            Debug.Log("Farkle detected.");
            EndTurn();
        }
    }


    private bool AllDiceStoppedRolling(List<DieRoller2D> diceList)
    {
        foreach (var d in diceList)
        {
            if (d.IsRolling())
            {
                return false;
            }
        }
        return true;
    }

    public void EndTurn()
    {
        Debug.Log(playerTurn ? "Player's turn ended." : "Opponent's turn ended.");
        isFarkle = false;

        if (playerTurn)
        {
            StartTurn(false); // Start opponent's turn
        }
        else
        {
            StartTurn(true); // Start player's turn
        }
    }
}
