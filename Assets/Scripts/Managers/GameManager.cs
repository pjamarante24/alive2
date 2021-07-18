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

    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private bool gameLost = false;

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        Time.timeScale = 0;
        // SceneManager.LoadScene("Main");
    }

    private IEnumerator RoundStarting()
    {
        Setup();

        messageText.text = "Round 1";

        yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        messageText.text = string.Empty;

        while (PlayerIsAlive())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        if (gameLost) messageText.text = "Game Over";
        else messageText.text = "Round End";

        yield return endWait;
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
