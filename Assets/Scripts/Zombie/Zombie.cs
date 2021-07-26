using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float health = 100f;
    public float damage = 15f;
    public int scorePerZombie = 400;
    public ZombieFollow zombieFollow;
    public Player player;
    public ZombieManager zombieManager;
    public bool isDead = false;

    void Start()
    {
        if (!zombieFollow) zombieFollow = gameObject.GetComponent<ZombieFollow>();
        if (!player) player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
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
        if (player) player.AddScore(scorePerZombie);
        zombieFollow.Kill();
        zombieManager.ZombieKilled(gameObject);
        Destroy(gameObject, 3f);
    }

    public void OnAttack()
    {
        if (zombieFollow.TargetIsClose())
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth) playerHealth.TakeDamage(damage);
        }
    }

    public void SetMultipliers(float speedMultiplier, float healthMultiplier, float damageMultiplier)
    {
        health = health * healthMultiplier;
        damage = damage * damageMultiplier;
        zombieFollow.agent.speed = zombieFollow.agent.speed * speedMultiplier;
    }
}
