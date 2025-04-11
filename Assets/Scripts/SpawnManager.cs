using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnPrefabs;
    
    private Vector3 spawnPosition;

    private float spawnTimer;
    private readonly float spawnTimerMax = 2f;

    void Start()
    {
        //spawnPosition = new Vector3(25f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(!Player.gameOver && spawnTimer >= spawnTimerMax)
        {
            int random = Random.Range(0, spawnPrefabs.Length);
            GameObject spawnPrefab = spawnPrefabs[random];
			spawnPosition = new Vector3(25f, 0, spawnPrefab.transform.position.z);
			Instantiate(spawnPrefab, spawnPosition, spawnPrefab.transform.rotation);
            spawnTimer = 0;
        }
    }
}
