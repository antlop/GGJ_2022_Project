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

    public Sprite AbilityIcon { get; set; }

    public void MakeAttack()
    {
        if ( spawnedAttackObject == null && combatAttackColliderPrefab) {
            spawnedAttackObject = Instantiate(combatAttackColliderPrefab, MeleeAttackSpawnPoint.position, MeleeAttackSpawnPoint.rotation);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1,4);
            Destroy(spawnedAttackObject, 0.15f);
            cdTimer = 0;
        }
    }

    public void MakeAttackTowardMouseClick(Vector3 mouseClickPoint)
    {
        if ( spawnedAttackObject == null && combatAttackColliderPrefab)
        {
            spawnedAttackObject = Instantiate(combatAttackColliderPrefab, MeleeAttackSpawnPoint.position, MeleeAttackSpawnPoint.rotation);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1, 4);
            spawnedAttackObject.transform.Rotate(transform.parent.position, Vector3.Angle(spawnedAttackObject.transform.forward, (mouseClickPoint - spawnedAttackObject.transform.position)));
            Destroy(spawnedAttackObject, 0.15f);
            cdTimer = 0;
        }
    }

    public bool IsAttackOffCooldown()
    {
        return cdTimer == CooldownTime;
    }

    private void LateUpdate()
    {

        if (cdTimer < CooldownTime)
        {
            cdTimer += Time.deltaTime;
            if (cdTimer >= CooldownTime)
            {
                cdTimer = CooldownTime;
            }

            if (CooldownSlider)
            {
                CooldownSlider.value = 1 - (cdTimer / CooldownTime);
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
