using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFromWeapon : MonoBehaviour
{
    [SerializeField] GameObject attackToSpawn;
    [SerializeField] GameObject spawnPoint;

    public void SpawnPrefab()
    {
        GameObject spawnedObj = Instantiate(attackToSpawn);
        spawnedObj.transform.position = spawnPoint.transform.position;
    }
}
