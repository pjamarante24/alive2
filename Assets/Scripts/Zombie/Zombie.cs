using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float health = 100f;
    public ZombieFollow zombieFollow;
    void Start()
    {
        if (!zombieFollow) zombieFollow = gameObject.GetComponent<ZombieFollow>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Kill();

    }
    private void Kill()
    {
        zombieFollow.Kill();
        Destroy(gameObject, 5f);
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
