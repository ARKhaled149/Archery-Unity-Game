using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is used to switch between scenes and quit the game.
public class SceneSwitcher : MonoBehaviour
{
    // Loads a new scene based on the scene name provided
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quits the application or stops play mode if in the Unity editor
    public void QuitGame()
    {
#if UNITY_EDITOR
        // If the Unity editor is running, stop playing
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If built application is running, quit the application
        Application.Quit();
#endif
    }
}