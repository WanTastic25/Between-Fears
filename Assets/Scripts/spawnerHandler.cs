using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerHandler : MonoBehaviour
{
    public GameObject playerSpawnPoint;
    public GameObject enemySpawnPoint;
    public PlayerController player;
    public EnemyAI enemy;

    public GameObject medkit;
    public GameObject battery;
    public Vector2 xBounds; // X bounds for the spawn area
    public Vector2 zBounds; // Z bounds for the spawn area
    public float spawnHeight = 0.5f; 
    public float spawnInterval = 60f; //every minute

    private void Start()
    {
        player.transform.position = playerSpawnPoint.transform.position;
        enemy.transform.position = enemySpawnPoint.transform.position;
        StartCoroutine(SpawnStuffAtIntervals());
    }

    private IEnumerator SpawnStuffAtIntervals()
    {
        while (true)
        {
            SpawnMedkit();
            SpawnBattery();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnMedkit()
    {
        float randomX = Random.Range(xBounds.x, xBounds.y);
        float randomZ = Random.Range(zBounds.x, zBounds.y);

        Vector3 spawnPos = new Vector3(randomX, spawnHeight, randomZ);

        Instantiate(medkit, spawnPos, Quaternion.identity);
    }

    public void SpawnBattery()
    {
        float randomX = Random.Range(xBounds.x, xBounds.y);
        float randomZ = Random.Range(zBounds.x, zBounds.y);

        Vector3 spawnPos = new Vector3(randomX, 3.129244e-05f, randomZ);

        Instantiate(battery, spawnPos, Quaternion.identity);
    }
}
