using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameManager gameManager;
    public ZombieManager zombieManager;
    public AudioMixer audioMixer;
    public Text roundText;
    public Text zombiesLeftText;
    public Text zombiesKilledText;
    private bool isPaused = false;

    private void Start()
    {
        if (!gameManager) gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (!zombieManager) zombieManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ZombieManager>();
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
        zombiesLeftText.text = zombieManager.zombiesLeft().ToString();
        zombiesKilledText.text = CalculateZombiesKilled(gameManager.round, zombieManager.initialZombieCount, zombieManager.zombiesLeft()).ToString();
    }

    float CalculateZombiesKilled(int round, int initialZombieCount, int zombiesLeft)
    {
        float a = initialZombieCount;
        float d = ((a * 2) - a);
        float n = round;

        float totalZombies = (n / 2) * ((2 * a) + (n - 1) * d);

        Debug.Log((n / 2) + " * ((" + (2 * a) + ")" + ((n - 1) * d) + ") = " + totalZombies);

        return totalZombies - zombiesLeft;
    }

    public void OnExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }
}
