using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float health = 100f;
    public int scorePerZombie = 400;
    public ZombieFollow zombieFollow;
    public PlayerScore playerScore;
    public ZombieManager zombieManager;
    public bool isDead = false;

    void Start()
    {
        if (!zombieFollow) zombieFollow = gameObject.GetComponent<ZombieFollow>();
        if (!playerScore) playerScore = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerScore>();
        if (!zombieManager) zombieManager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<ZombieManager>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0 && !isDead) Kill();

    }
    private void Kill()
    {
        isDead = true;
        if (playerScore) playerScore.AddScore(scorePerZombie);
        zombieFollow.Kill();
        zombieManager.ZombieKilled(gameObject);
        Destroy(gameObject, 3f);
    }

    public void OnAttack()
    {
        if (zombieFollow.TargetIsClose())
        {
            PlayerHealth player = zombieFollow.target.GetComponent<PlayerHealth>();
            if (player) player.TakeDamage(15f);
        }
    }
}
