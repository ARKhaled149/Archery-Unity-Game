using TMPro;
using UnityEngine;

// This script manages the end scene, displaying the final score.
public class EndSceneManager : MonoBehaviour
{
    public TMP_Text finalScoreText; // UI element for displaying the final score

    void Start()
    {
        // Calculate the total score from level 1 and level 2
        int totalScore = GameManager.level1Score + GameManager.level2Score;
        finalScoreText.text = "Final Score: " + totalScore; // Display the total score
    }
}