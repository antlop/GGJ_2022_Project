using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public GameObject MovingUnit;
    public LayerMask ClickToMoveLayer;

    private bool ShouldCheckForTargetPosition = false;
    private Vector3 TargetPosition = Vector3.zero;
    private NavMeshAgent NMAgent;
    private Vector3 TargetPoint;
    private Animator Anim;
    public bool PauseInputUntilDestinationIsReached = false;

    //private PlayerStats ThePlayerStats;

   // public CanvasManager CanvasManagerObject;

    // Start is called before the first frame update
    void Start()
    {
        NMAgent = MovingUnit.GetComponent<NavMeshAgent>();
        Anim = MovingUnit.GetComponentInChildren<Animator>();
    //    ThePlayerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
    //    if( ThePlayerStats.PlayerIsDead ) { return; }
    //    if( CanvasManagerObject.ShowingWindow ) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            ShouldCheckForTargetPosition = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ShouldCheckForTargetPosition = false;
        }


        if (PauseInputUntilDestinationIsReached && NMAgent)
        {
            ShouldCheckForTargetPosition = false;
        }


        if (ShouldCheckForTargetPosition)
        {
            if (FindTargetPosition())
            {
                NMAgent.SetDestination(TargetPoint);
                NMAgent.speed = 7f;
                Anim.SetBool("Moving", true);
            }
        }

        if (NMAgent && !NMAgent.hasPath)
        {
            Anim.SetBool("Moving", false);
        }


        if (Input.GetMouseButtonDown(1))
        {
            TurnToMouseClick();
            Anim.SetTrigger("Attack1Trigger");
        }

        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack1"))
        {
            if(this.gameObject.activeSelf == true && IsAgentOnNavMesh(this.gameObject))
                NMAgent.isStopped = true;
        }
        else
        {
            if (this.gameObject.activeSelf == true && IsAgentOnNavMesh(this.gameObject))
                NMAgent.isStopped = false;
        }
    }

    float onMeshThreshold = 3;

    public bool IsAgentOnNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }

    void TurnToMouseClick()
    {
        Vector3 targetPos = GetMouseClickPoint();
        if (targetPos != transform.position)
        {
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
        }
    }

    bool FindTargetPosition()
    {
        Vector3 targetPos = GetMouseClickPoint();
        if (targetPos != transform.position)
        {
            TargetPoint = targetPos;
            return true;
        }
        return false;
    }

    Vector3 GetMouseClickPoint()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, ClickToMoveLayer))
        {
            return hit.point;
        }
        return transform.position;
    }

    public void SetPausePlayerInputUntilDestination(bool paused)
    {
        PauseInputUntilDestinationIsReached = paused;
    }
    public bool GetPausePlayerInputUntilDestination()
    {
        return PauseInputUntilDestinationIsReached;
    }
}
