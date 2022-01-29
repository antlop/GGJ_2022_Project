using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningBeam : MonoBehaviour, IAbility
{
    public GameObject combatAttackColliderPrefab;
    public Transform MeleeAttackSpawnPoint;

    public LayerMask ClickToMoveLayer;

    public float CooldownTime = 1.0f;
    public float cdTimer = 0f;

    GameObject spawnedAttackObject;

    Slider CooldownSlider;

    private KeyCode ActivationKey;
    private bool IsRightMouseButton = false;

    Transform player;

    public Sprite IconForActionBar;

    public Vector2Int DamageRange = Vector2Int.zero;

    public Sprite AbilityIcon { get { return IconForActionBar; } set { IconForActionBar = value; } }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MeleeAttackSpawnPoint = player.Find("AttackingPoint");
    }

    public void MakeAttack()
    {
        if ( spawnedAttackObject == null && combatAttackColliderPrefab) {
            spawnedAttackObject = Instantiate(combatAttackColliderPrefab, MeleeAttackSpawnPoint.position, MeleeAttackSpawnPoint.rotation);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = DamageRange;
            Destroy(spawnedAttackObject, 0.25f);
            cdTimer = 0;
        }
    }

    public void MakeAttackTowardMouseClick(Vector3 mouseClickPoint)
    {
        if ( spawnedAttackObject == null && combatAttackColliderPrefab)
        {
            spawnedAttackObject = Instantiate(combatAttackColliderPrefab, MeleeAttackSpawnPoint.position, MeleeAttackSpawnPoint.rotation);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = DamageRange;
            spawnedAttackObject.transform.Rotate(player.position, Vector3.Angle(spawnedAttackObject.transform.forward, (mouseClickPoint - spawnedAttackObject.transform.position)));
            Destroy(spawnedAttackObject, 0.25f);
            cdTimer = 0;
        }
    }

    public bool IsAttackOffCooldown()
    {
        return cdTimer == CooldownTime;
    }

    private void LateUpdate()
    {
        if (((IsRightMouseButton && Input.GetMouseButton(1)) || Input.GetKeyUp(ActivationKey)) && IsAttackOffCooldown())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, ClickToMoveLayer))
            {
                MakeAttackTowardMouseClick(hit.point);
            }
            else
            {
                MakeAttack();
            }

            player.GetComponent<Animator>().SetTrigger("Bash");
        }






        if (cdTimer < CooldownTime)
        {
            cdTimer += Time.deltaTime * player.GetComponent<Character_Core_Manager>().BaseStats.Haste;
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
        ActivationKey = key;
        if( ActivationKey == KeyCode.Caret)
        {
            IsRightMouseButton = true;
        }
    }

    public void SetCooldownSlider(Slider _slider)
    {
        CooldownSlider = _slider;
    }
}
