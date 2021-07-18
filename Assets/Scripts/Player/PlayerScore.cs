using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public int initialScore = 400;
    public int currentScore;
    public Text scoreText;

    void OnEnable()
    {
        currentScore = initialScore;
        UpdateUI();
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UpdateUI();
    }

    public bool SubtractScore(int scoreToSubtract)
    {
        if (currentScore - scoreToSubtract >= 0)
        {
            currentScore -= scoreToSubtract;
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateUI()
    {
        scoreText.text = currentScore.ToString();
    }
}
