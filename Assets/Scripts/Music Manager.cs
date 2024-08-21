using UnityEngine;

// This script manages background music, ensuring it persists across scene changes.
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null; // Singleton instance

    public static MusicManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Implement singleton pattern to ensure only one instance of MusicManager exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // Destroy duplicate instances
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject); // Persist this object across scene loads
    }
}