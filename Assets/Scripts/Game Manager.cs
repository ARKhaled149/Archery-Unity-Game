using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

// This script manages the game's score, arrow count, and scene transitions.
public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText; // UI element for displaying the score
    public TMP_Text arrowsText; // UI element for displaying arrows left
    private int score = 0; // Current score
    private int arrowsLeft = 10; // Initial number of arrows

    public static int level1Score = 0; // Static field to store level 1 score
    public static int level2Score = 0; // Static field to store level 2 score

    private void Start()
    {
        UpdateScoreUI(); // Update score display on start
        UpdateArrowsUI(); // Update arrows display on start
    }

    public void AddScore(int points)
    {
        score += points; // Add points to the score
        UpdateScoreUI(); // Update the score display
    }

    public void UseArrow()
    {
        if (arrowsLeft > 0)
        {
            arrowsLeft--; // Decrement the number of arrows left
            UpdateArrowsUI(); // Update the arrows display
        }
        CheckEndOfLevel(); // Check if the level should end
    }

    private void CheckEndOfLevel()
    {
        if (arrowsLeft == 0)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex == 1) // If on Level 1
            {
                level1Score = score; // Store the level 1 score
                StartCoroutine(LoadSceneWithDelay(2, 3f)); // Load Level 2 after a delay
            }
            else if (currentSceneIndex == 2) // If on Level 2
            {
                level2Score = score; // Store the level 2 score
                StartCoroutine(LoadSceneWithDelay(3, 3f)); // Load End Scene after a delay
            }
        }
    }

    private IEnumerator LoadSceneWithDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay
        SceneManager.LoadScene(sceneIndex); // Load the specified scene
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score; // Update the score UI text
    }

    private void UpdateArrowsUI()
    {
        arrowsText.text = "Arrows: " + arrowsLeft; // Update the arrows UI text
    }

    public bool HasArrowsLeft()
    {
        return arrowsLeft > 0; // Check if there are arrows left
    }
}