using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public float startDelay = 3f;
    public float endDelay = 3f;
    public Text messageText;
    public Text zombiesLeftText;
    public Transform[] spawnPoints;
    public GameObject zombiePrefab;
    public int zombieCount = 10;

    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private bool gameLost = false;
    private ZombieManager[] zombies;

    private void Start()
    {
        zombies = new ZombieManager[zombieCount];

        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        SceneManager.LoadScene("Main");
    }


    private IEnumerator RoundStarting()
    {
        Setup();

        messageText.text = "Game Starting!";

        yield return startWait;
    }


    private IEnumerator RoundPlaying()
    {
        messageText.text = string.Empty;

        while (PlayerIsAlive() && ZombiesLeft() > 0)
        {
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        if (gameLost)
            messageText.text = "Game Over!";
        else
            messageText.text = "You Win!";

        yield return endWait;
    }

    private bool PlayerIsAlive()
    {
        bool isDead = player.GetComponent<HealthPlayer>().isDead;
        if (isDead) gameLost = true;
        return !isDead;
    }

    private int ZombiesLeft()
    {
        int zombiesLeft = 0;

        for (int i = 0; i < zombies.Length; i++)
        {
            if (zombies[i].instance && zombies[i].instance.activeSelf)
                zombiesLeft++;
        }

        UpdateZombiesLeftUI(zombiesLeft);

        return zombiesLeft;
    }

    private void Setup()
    {
        gameLost = false;
        SpawnAllZombies();
    }

    private void SpawnAllZombies()
    {
        for (int i = 0; i < zombieCount; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            ZombieManager manager = new ZombieManager();
            manager.instance = (GameObject)Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
            zombies[i] = manager;
        }
    }

    private void UpdateZombiesLeftUI(int left)
    {
        if (zombiesLeftText != null)
            zombiesLeftText.text = left.ToString();
    }

}
