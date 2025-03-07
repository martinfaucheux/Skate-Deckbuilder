using TMPro;
using UnityEngine;

public class RunStats : MonoBehaviour
{
    public TMP_Text roundText;
    public void UpdateRound(int currentRound, int maxRound)
    {
        roundText.text = $"{currentRound}/{maxRound}";
    }

    public TMP_Text handText;
    public void UpdateHand(int currentHand, int maxHand)
    {
        handText.text = $"{currentHand}/{maxHand}";
    }

    public TMP_Text scoreText;
    public void UpdateScore(int currentScore, int maxScore)
    {
        scoreText.text = $"{currentScore}/{maxScore}";
    }
}
