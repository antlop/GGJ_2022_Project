using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningSpin : MonoBehaviour, IAbility
{
    public GameObject combatAOEColliderPrefab;

    public float CooldownTime = 15.0f;
    public float cdTimer = 0f;

    private KeyCode ActivationKey;

    [SerializeField] private bool Spinning = false;

    GameObject spawnedAttackObject;

    Slider CooldownSlider;

    public ParticleSystem SpinFX;

    public void MakeAttack()
    {
        if (spawnedAttackObject == null && combatAOEColliderPrefab)
        {
            Vector3 spawnPoint = transform.parent.position;
            spawnPoint.y = 2f;

            spawnedAttackObject = Instantiate(combatAOEColliderPrefab, spawnPoint, Quaternion.identity);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1, 3);
        }
    }

    public void MakeAttackTowardMouseClick(Vector3 mouseClickPoint)
    {
        if (combatAOEColliderPrefab)
        {
            Vector3 spawnPoint = transform.parent.position;
            spawnPoint.y = 2f;

            spawnedAttackObject = Instantiate(combatAOEColliderPrefab, spawnPoint, Quaternion.identity);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1, 4);
        }
    }

    public bool IsAttackOffCooldown()
    {
        return cdTimer <= 0f;
    }

    private void LateUpdate()
    {
        if( IsAttackOffCooldown() && Input.GetKeyUp(ActivationKey))
        {
            Spinning = true;
            MakeAttack();
            transform.parent.GetComponent<Animator>().SetTrigger("LightningSpin");
            Destroy(Instantiate(SpinFX, new Vector3(0, 2, 0) + transform.parent.position, Quaternion.identity, transform.parent), 0.9f);
        }

        if (Spinning)
        {
            cdTimer += Time.deltaTime;
            CooldownSlider.value = 1 - (cdTimer / CooldownTime);

            if (cdTimer >= CooldownTime)
            {
                CooldownSlider.value = 0;
                cdTimer = 0;
                DestroyImmediate(spawnedAttackObject);
                spawnedAttackObject = null;
                Spinning = false;
            }
        }
    }

    public void SetActivationKey(KeyCode key)
    {
        ActivationKey = key;
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
