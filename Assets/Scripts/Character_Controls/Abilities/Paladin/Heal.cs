using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heal : MonoBehaviour, IAbility
{
    private KeyCode ActivationKey;
    private bool IsRightMouseButton = false;
    public Sprite IconForActionBar;
    Slider CooldownSlider;

    GameObject Player;

    public float CooldownTime = 1.0f;
    public float cdTimer = 0f;

    public Vector2Int HealRange = Vector2Int.zero;
    public ParticleSystem HealFX;

    public Sprite AbilityIcon { get { return IconForActionBar; } set { IconForActionBar = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if( cdTimer < CooldownTime )
        {
            cdTimer += Time.deltaTime * Player.GetComponent<Character_Core_Manager>().BaseStats.Haste;
            if( cdTimer >= CooldownTime )
            {
                cdTimer = CooldownTime;
            }

            if (CooldownSlider)
            {
                CooldownSlider.value = 1 - (cdTimer / CooldownTime);
            }
        }


        if (((IsRightMouseButton && Input.GetMouseButton(1)) || Input.GetKeyUp(ActivationKey)) && IsAttackOffCooldown())
        {
            cdTimer = 0f;
            // TODO: Heal the player
            Player.GetComponent<Animator>().SetTrigger("SelfCast");
            if(HealFX)
            {
                Destroy(Instantiate(HealFX, Player.transform), 2.5f);
            }
            Player.GetComponent<Character_Core_Manager>().BaseStats.AdjustCurrentHealth(Random.Range(HealRange.x, HealRange.y+1));
        }
    }
    public bool IsAttackOffCooldown()
    {
        return cdTimer == CooldownTime;
    }

    public void SetActivationKey(KeyCode key)
    {
        ActivationKey = key;
        if (ActivationKey == KeyCode.Caret)
        {
            IsRightMouseButton = true;
        }
    }

    public void SetCooldownSlider(Slider _slider)
    {
        CooldownSlider = _slider;
    }

}
