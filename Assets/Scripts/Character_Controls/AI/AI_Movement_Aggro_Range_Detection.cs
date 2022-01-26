using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement_Aggro_Range_Detection : MonoBehaviour
{
    AI_Movement_Chase chaseBehavior;
    public float Aggro_Range = 5f;
    private Transform PlayerTransform;

    private void Start()
    {
        chaseBehavior = GetComponent<AI_Movement_Chase>();

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if( PlayerTransform )
        {
            RaycastHit hit; 
            int layerMask = ~LayerMask.GetMask("Combat");
            if (Physics.Linecast(transform.position, PlayerTransform.position, out hit, layerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    chaseBehavior.FoundTarget(PlayerTransform.gameObject);
                } else
                {
                    chaseBehavior.FoundTarget(null);
                }
            }
        }
    }
}
