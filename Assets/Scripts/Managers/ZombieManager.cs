using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public GameObject[] zombiesPrefab;
    public GameObject[] spawnPoints;
    private List<GameObject> zombies = new List<GameObject>();

    public int initialZombieCount = 3;
    public int currentZombieCount;

    public void Setup(int round = 1)
    {
        currentZombieCount = initialZombieCount * round;

        zombies.Clear();
        StartCoroutine(SpawnZombies(currentZombieCount));
    }

    IEnumerator SpawnZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(Random.Range(2, 6));
            SpawnZombie(i);
        }
    }

    void SpawnZombie(int index)
    {
        GameObject zombiePrefab = zombiesPrefab[Random.Range(0, zombiesPrefab.Length)];
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        zombies.Add(zombie);
    }

    public void ZombieKilled(GameObject zombie)
    {
        if (zombies.Contains(zombie))
        {
            zombies.Remove(zombie);

            currentZombieCount--;
        }
    }

    public int zombiesLeft()
    {
        return currentZombieCount;
    }
}
