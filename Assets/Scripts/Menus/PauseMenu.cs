using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // public AudioMixer audioMixer;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // On Escape, toggle pause
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        // audioMixer.SetFloat("EffectsVolume", 0);
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        // audioMixer.SetFloat("EffectsVolume", -80);
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void OnExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }
}
