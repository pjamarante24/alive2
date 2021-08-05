using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameManager gameManager;
    public AudioMixer audioMixer;
    public Text roundText;
    private bool isPaused = false;

    private void Start()
    {
        if (!gameManager) gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

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
        audioMixer.SetFloat("EffectsVolume", 0);
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        UpdateUI();
        audioMixer.SetFloat("EffectsVolume", -80);
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    void UpdateUI()
    {
        roundText.text = gameManager.round.ToString();
    }

    public void OnExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }
}
