using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public int round = 1;
    public float startDelay = 3f;
    public float endDelay = 3f;
    public Text messageText;
    private bool gameLost = false;
    public ZombieManager zombieManager;

    private void Start()
    {
        if (!zombieManager) zombieManager = GetComponent<ZombieManager>();

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStarting());
        yield return StartCoroutine(RoundLoop());
        yield return StartCoroutine(GameEnding());
    }

    private IEnumerator GameStarting()
    {
        Setup();
        messageText.text = "Game Starting";

        yield return new WaitForSeconds(3f);
    }

    private IEnumerator RoundLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());

        if (!gameLost)
        {
            yield return StartCoroutine(RoundEnding());
            yield return StartCoroutine(RoundLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        messageText.text = "Round " + round;

        yield return new WaitForSeconds(3f);

        messageText.text = string.Empty;

        yield return new WaitForSeconds(5f);

        zombieManager.Setup(round);
    }

    private IEnumerator RoundPlaying()
    {
        while (PlayerIsAlive() && zombieManager.zombiesLeft() > 0)
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        messageText.text = "Round End";
        round++;

        yield return new WaitForSeconds(3f);
    }

    private IEnumerator GameEnding()
    {
        messageText.text = "Game Over";

        yield return new WaitForSeconds(3f);
    }

    private bool PlayerIsAlive()
    {
        bool isDead = player.GetComponent<PlayerHealth>().isDead;
        if (isDead) gameLost = true;
        return !isDead;
    }

    private void Setup()
    {
        gameLost = false;
    }
}
