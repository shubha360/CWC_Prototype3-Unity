using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    private Vector3 spawnPos = new Vector3(22, 0, 0);

    private float startDelay = 3;
    private float interval = 2;

    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        InvokeRepeating("spawnObstacle", startDelay, interval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnObstacle()
    {
        if (!playerControllerScript.gameOver && playerControllerScript.startGame)
        {
            GameObject nextSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            Instantiate(nextSpawn, spawnPos, nextSpawn.transform.rotation);
        }
    }
}
