using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement_Chase : MonoBehaviour
{
    private NavMeshAgent NavAgent;

    [SerializeField] private GameObject ChaseTarget;
    private Vector3 LastGoodPosition;

    public float partialPositioningBuffer = 0.5f;

    private float TimeToRecheck = 1.5f;
    private float TimeToRecheckBucket = 0;

    private void Start()
    {
        LastGoodPosition = new Vector3(0,-9999,0);
        NavAgent = GetComponent<NavMeshAgent>();
    }

    public void FoundTarget(GameObject target)
    {
        ChaseTarget = target;
        if (ChaseTarget )
        {
            LastGoodPosition = ChaseTarget.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (ChaseTarget || LastGoodPosition.y > -9000f)
        {
            TimeToRecheckBucket += Time.deltaTime;
            if (TimeToRecheckBucket >= TimeToRecheck)
            {
                if (ChaseTarget)
                {
                    LastGoodPosition = ChaseTarget.transform.position;
                }
                NavAgent.SetDestination(LastGoodPosition);
            }
        }
    }
}
