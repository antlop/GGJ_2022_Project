using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRacastBurstSpawner : MonoBehaviour
{
    public GameObject BaseEnemy;

    public int EnemiesToSpawn = 0;
    public float SpawnDelay = 0.1f;

    private int MaxAttempts = 0;

    // Start is called before the first frame update
    void Start()
    {
        MaxAttempts = EnemiesToSpawn * 3;
        Invoke("Spawn", SpawnDelay);
    }

    void Spawn()
    {
        int currEnemiesSpawned = 0;
        for( int i = 0; i < MaxAttempts; i++)
        {
            RaycastHit hit;
            float x = Random.Range(-180, 180);
            float z = Random.Range(-180, 180);
            Vector3 dir = new Vector3(x, 0.5f, z).normalized;
            if ( Physics.Raycast(transform.position, dir, out hit))
            {
                if( hit.transform.tag != "SpawnedEnemy")
                {
                    GameObject obj = Instantiate(BaseEnemy, Vector3.zero, Quaternion.identity);
                    obj.SetActive(false);
                    obj.transform.position = hit.transform.position - (dir * 1.5f);
                    obj.SetActive(true);
                    obj.tag = "SpawnedEnemy";
                    currEnemiesSpawned++;
                }
            }

            if( EnemiesToSpawn == currEnemiesSpawned )
            {
                break;
            }
        }

        Destroy(gameObject);
    }
}
