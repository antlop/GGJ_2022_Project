using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlessingOfCelerity : MonoBehaviour, IAbility
{
    private KeyCode ActivationKey;
    private bool IsRightMouseButton = false;
    public Sprite IconForActionBar;
    Slider CooldownSlider;

    GameObject Player;

    public float CooldownTime = 1.0f;
    public float cdTimer = 0f;

    public float HasteBuff = 0.25f;
    public float BuffDuration = 1f;
    public float BuffBucket = 0f;
    public bool Buffed = false;

    public ParticleSystem InitialCastFX;
    public ParticleSystem DurationFX;

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
            cdTimer += Time.deltaTime;
            if( cdTimer >= CooldownTime )
            {
                cdTimer = CooldownTime;
            }

            if (CooldownSlider)
            {
                CooldownSlider.value = 1 - (cdTimer / CooldownTime);
            }
        }

        if( Buffed )
        {
            BuffBucket += Time.deltaTime;
            if( BuffBucket >= BuffDuration)
            {
                Player.GetComponent<Character_Core_Manager>().BaseStats.Haste = 1;
                Buffed = false;
            }
        }

        if (((IsRightMouseButton && Input.GetMouseButton(1)) || Input.GetKeyUp(ActivationKey)) && IsAttackOffCooldown())
        {
            cdTimer = 0;

            // TODO: Heal the player
            Player.GetComponent<Animator>().SetTrigger("SelfCast");
            if(InitialCastFX)
            {
                Destroy(Instantiate(InitialCastFX, Player.transform), 1.5f);
            }
            if (DurationFX)
            {
                Destroy(Instantiate(DurationFX, Player.transform), BuffDuration);
            }
            Player.GetComponent<Character_Core_Manager>().BaseStats.Haste = 1 + HasteBuff;
            Buffed = true;
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
