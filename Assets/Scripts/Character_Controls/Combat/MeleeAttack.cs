using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeAttack : MonoBehaviour, IAbility
{
    public GameObject combatAttackColliderPrefab;
    public Transform MeleeAttackSpawnPoint;

    public float CooldownTime = 1.0f;
    public float cdTimer = 0f;

    GameObject spawnedAttackObject;

    Slider CooldownSlider;

    public void MakeAttack()
    {
        if ( spawnedAttackObject == null && combatAttackColliderPrefab) {
            spawnedAttackObject = Instantiate(combatAttackColliderPrefab, MeleeAttackSpawnPoint.position, MeleeAttackSpawnPoint.rotation);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1,4);
        }
    }

    public void MakeAttackTowardMouseClick(Vector3 mouseClickPoint)
    {
        if ( spawnedAttackObject == null && combatAttackColliderPrefab)
        {
            spawnedAttackObject = Instantiate(combatAttackColliderPrefab, MeleeAttackSpawnPoint.position, MeleeAttackSpawnPoint.rotation);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1, 4);
            spawnedAttackObject.transform.Rotate(transform.parent.position, Vector3.Angle(spawnedAttackObject.transform.forward, (mouseClickPoint - spawnedAttackObject.transform.position)));
        }
    }

    public bool IsAttackOffCooldown()
    {
        return cdTimer <= 0f;
    }

    private void LateUpdate()
    {
        if (spawnedAttackObject)
        {
            cdTimer += Time.deltaTime;
            CooldownSlider.value = 1 - (cdTimer / CooldownTime);

            if (cdTimer >= CooldownTime)
            {
                CooldownSlider.value = 0;
                cdTimer = 0;
                DestroyImmediate(spawnedAttackObject);
                spawnedAttackObject = null;
            }
        }
    }

    public void SetActivationKey(KeyCode key)
    {
        throw new System.NotImplementedException();
    }

    public void Activate()
    {
        throw new System.NotImplementedException();
    }

    public void SetCooldownSlider(Slider _slider)
    {
        CooldownSlider = _slider;
    }
}
