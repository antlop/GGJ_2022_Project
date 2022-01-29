using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bash : MonoBehaviour, IAbility
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
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1,4);
        }
    }

    public void MakeAttackTowardMouseClick(Vector3 mouseClickPoint)
    {
        if ( spawnedAttackObject == null && combatAttackColliderPrefab)
        {
            spawnedAttackObject = Instantiate(combatAttackColliderPrefab, MeleeAttackSpawnPoint.position, MeleeAttackSpawnPoint.rotation);
            spawnedAttackObject.GetComponent<AttackCollisionDetection>().DamageToApply = new Vector2Int(1, 4);
            spawnedAttackObject.transform.Rotate(player.position, Vector3.Angle(spawnedAttackObject.transform.forward, (mouseClickPoint - spawnedAttackObject.transform.position)));
        }
    }

    public bool IsAttackOffCooldown()
    {
        return cdTimer <= 0f;
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






        if (spawnedAttackObject)
        {
            cdTimer += Time.deltaTime;
            if (CooldownSlider)
            {
                CooldownSlider.value = 1 - (cdTimer / CooldownTime);
            }

            if (cdTimer >= CooldownTime)
            {
                if (CooldownSlider)
                {
                    CooldownSlider.value = 0;
                }
                cdTimer = 0;
                DestroyImmediate(spawnedAttackObject);
                spawnedAttackObject = null;
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
