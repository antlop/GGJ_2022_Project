using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRacastBurstSpawner : MonoBehaviour
{
    public GameObject BaseEnemy;

    public int EnemiesToSpawn = 0;
    public float SpawnDelay = 0.5f;

    private int MaxAttempts = 0;

    private ObjectPool _BaseEnemyPool;

    // Start is called before the first frame update
    void Start()
    {
        _BaseEnemyPool = GameObject.Find("Enemy_Object Pool").GetComponent<ObjectPool>();
        MaxAttempts = EnemiesToSpawn * 3;
        Invoke("Spawn", SpawnDelay);
    }

    void Spawn()
    {
        if( !_BaseEnemyPool )
        {
            _BaseEnemyPool = GameObject.Find("Enemy_Object Pool").GetComponent<ObjectPool>();
        }

        int currEnemiesSpawned = 0;
        for( int i = 0; i < MaxAttempts; i++)
        {
            //TODO: I am not sure this is doing what we think it's doing.   Going to put in some debug draw calls to test this la
            RaycastHit hit;
            float x = Random.Range(-180, 180);
            float z = Random.Range(-180, 180);
            Vector3 dir = new Vector3(x, 0.5f, z).normalized;
            if ( Physics.Raycast(transform.position, dir, out hit))
            {
                if( hit.transform.tag != "SpawnedEnemy")
                {
                    GameObject obj = _BaseEnemyPool.GetObject("Base Enemy");
                    obj.SetActive(false);
                    obj.transform.position = hit.transform.position - (dir * 1.5f);
                    obj.SetActive(true);

                    BaseComponentController componentController = obj.GetComponent<BaseComponentController>();
                    if (componentController != null)
                    {
                        componentController.ActivateObject();
                    }
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
