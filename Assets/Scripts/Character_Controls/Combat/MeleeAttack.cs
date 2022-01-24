using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public GameObject combatAttackColliderPrefab;
    public Transform MeleeAttackSpawnPoint;

    int spawnCount = 0;
    GameObject spawnedAttackObject;

    public void MakeAttack()
    {
        if (spawnedAttackObject == null && combatAttackColliderPrefab) {
            spawnedAttackObject = Instantiate(combatAttackColliderPrefab, MeleeAttackSpawnPoint.position, MeleeAttackSpawnPoint.rotation);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1,4);
        }
    }

    private void LateUpdate()
    {
        if (spawnedAttackObject)
        {
            spawnCount++;
            if (spawnCount >= 90)
            {
                spawnCount = 0;
                DestroyImmediate(spawnedAttackObject);
                spawnedAttackObject = null;
            }
        }
    }
}