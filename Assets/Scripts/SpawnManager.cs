using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPrefab;
    
    private Vector3 spawnPosition;

    private float spawnTimer = 2f;
    private float spawnTimerMax = 2f;

    void Start()
    {
        spawnPosition = new Vector3(25f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(!Player.gameOver && spawnTimer >= spawnTimerMax)
        {
            Instantiate(spawnPrefab, spawnPosition, spawnPrefab.transform.rotation);
            spawnTimer = 0;
        }
    }
}
