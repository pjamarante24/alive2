using System;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [Serializable]
    public class Score
    {
        public int currentScore = 0;
        public int initialScore = 0;
        public Text scoreText;

    }

    public Score score;

    void OnEnable()
    {
        score.currentScore = score.initialScore;
        UpdateUI();
    }

    public void AddScore(int scoreToAdd)
    {
        score.currentScore += scoreToAdd;
        UpdateUI();
    }

    public bool SubtractScore(int scoreToSubtract)
    {
        if (score.currentScore - scoreToSubtract >= 0)
        {
            score.currentScore -= scoreToSubtract;
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
        score.scoreText.text = score.currentScore.ToString();
    }
}
