using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> zombiesPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    private List<GameObject> zombies = new List<GameObject>();
    private List<GameObject> nearestSpawnPoints;

    public int initialZombieCount = 3;
    public int currentZombieCount;

    void Start()
    {
        spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Spawnpoint"));

        InvokeRepeating("VerifyZombiesPosition", 10f, 10f);
    }

    public void Setup(int round = 1)
    {
        nearestSpawnPoints = GetNearestSpawnPoints(player.transform.position);
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

    List<GameObject> GetNearestSpawnPoints(Vector3 position)
    {
        List<GameObject> nearestSpawnPoints = new List<GameObject>();
        float nearestDistance = 100f;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            float distance = Vector3.Distance(position, spawnPoints[i].transform.position);
            if (distance < nearestDistance)
            {
                nearestSpawnPoints.Add(spawnPoints[i]);
            }
        }

        if (nearestSpawnPoints.Count > 0) return nearestSpawnPoints;
        else return spawnPoints;
    }

    void SpawnZombie(int index)
    {
        GameObject zombiePrefab = zombiesPrefab[Random.Range(0, zombiesPrefab.Count)];
        GameObject spawnPoint = nearestSpawnPoints[Random.Range(0, nearestSpawnPoints.Count)];
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

    void VerifyZombiesPosition()
    {
        nearestSpawnPoints = GetNearestSpawnPoints(player.transform.position);

        for (int i = 0; i < zombies.Count; i++)
        {
            if (zombies[i] == null) continue;

            float distance = Vector3.Distance(player.transform.position, zombies[i].transform.position);

            if (distance > 200f)
            {
                MoveZombieToNearestSpawnPoint(zombies[i]);
            }
        }
    }

    void MoveZombieToNearestSpawnPoint(GameObject zombie)
    {
        zombie.GetComponent<ZombieFollow>().agent.enabled = false;

        GameObject spawnPoint = nearestSpawnPoints[Random.Range(0, nearestSpawnPoints.Count)];
        zombie.transform.position = spawnPoint.transform.position;

        zombie.GetComponent<ZombieFollow>().agent.enabled = true;

        Debug.Log("Zombie " + zombie.name + " moved to " + spawnPoint.name);
    }
}
