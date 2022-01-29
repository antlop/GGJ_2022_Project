using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarManager : MonoBehaviour
{
    public Image ActionImage_Main;
    public Image ActionImage_One;
    public Image ActionImage_Two;

    private GameObject Player;

    private bool hasInitialized = false;

    // Start is called before the first frame update
    void LateUpdate()
    {
        if (!hasInitialized)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            CombatControls cc = Player.GetComponentInChildren<CombatControls>();

            if (cc)
            {
                ActionImage_Main.sprite = cc.MainAbility.GetComponent<IAbility>().AbilityIcon;
                ActionImage_One.sprite = cc.AbilityOne.GetComponent<IAbility>().AbilityIcon;
                ActionImage_Two.sprite = cc.AbilityTwo.GetComponent<IAbility>().AbilityIcon;
                hasInitialized = true;
            }
        }
    }
}
