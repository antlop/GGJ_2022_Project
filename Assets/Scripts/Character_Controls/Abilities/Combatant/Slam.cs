using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Slam : MonoBehaviour, IAbility
{
    public Rigidbody RB;
    public NavMeshAgent NMAgent;
    public LayerMask ClickToMoveLayer;

    [SerializeField] private bool Dashing = false;

    public float DashDuration = 1.25f;
    private float DashDurationBucket = 0f;

    public float DashSpeed = 50;

    private KeyCode ActivationKey;
    private bool IsRightMouseButton = false;

    Slider CooldownSlider;

    Transform player;

    public Sprite IconForActionBar;

    public Sprite AbilityIcon { get { return IconForActionBar; } set { IconForActionBar = value; } }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        NMAgent = player.GetComponentInChildren<NavMeshAgent>();
        RB = player.GetComponentInChildren<Rigidbody>();
    }

    public void ActivateDash(Vector3 MouseClickPosition)
    {
        player.GetComponent<Animator>().SetTrigger("Slam");
        player.GetComponent<ClickToMove>().enabled = false;
      
        Dashing = true;

        MouseClickPosition.y = player.position.y;
        player.position = MouseClickPosition;

        LeanTween.value(gameObject, player.position, MouseClickPosition, DashSpeed).setEasePunch();

        NMAgent.isStopped = true;
        NMAgent.ResetPath();
    }

    private void Update()
    {
        if ((IsRightMouseButton && Input.GetMouseButton(1)) || Input.GetKeyUp(ActivationKey))
        {
            Dashing = true;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.Log(ClickToMoveLayer);
            if (Physics.Raycast(ray, out hit, 9999, ClickToMoveLayer))

            {
                ActivateDash(hit.point);
            }
        }

        if( Dashing )
        {
            DashDurationBucket += Time.deltaTime;
            if( DashDurationBucket >= DashDuration )
            {
                DashDurationBucket = 0f;
                Dashing = false;

                player.GetComponent<ClickToMove>().enabled = true;
            }
        }

    }

    public void SetActivationKey(KeyCode key)
    {
        ActivationKey = key;
        if (ActivationKey == KeyCode.Caret)
        {
            IsRightMouseButton = true;
        }
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
