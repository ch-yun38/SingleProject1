using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject zomPre;
    public Transform playerPose;
    private float spawnTime;
    private float spawnInterver = 3f;
    //private float spawnNum = 3;
    private float spawnRadi = 6f;

    private void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime > spawnInterver)
        {
            SpawnMonster();
            spawnTime = 0;
        }
    }

    private void SpawnMonster()
    {
        Vector3 spawnPose = new Vector3(playerPose.position.x + spawnRadi, playerPose.position.y, playerPose.position.z + spawnRadi);

        Instantiate(zomPre, spawnPose, Quaternion.identity);
    }


}
