using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Dash : MonoBehaviour, IAbility
{
    public Rigidbody RB;
    public NavMeshAgent NMAgent;
    public LayerMask ClickToMoveLayer;

    [SerializeField] private bool Dashing = false;

    public float DashDuration = 1.25f;
    private float DashDurationBucket = 0f;

    public float DashSpeed = 50;

    private KeyCode ActivationKey;

    Slider CooldownSlider;

    // Start is called before the first frame update
    void Start()
    {
        NMAgent = transform.parent.GetComponentInChildren<NavMeshAgent>();
        RB = transform.parent.GetComponentInChildren<Rigidbody>();
    }

    public void ActivateDash(Vector3 MouseClickPosition)
    {

        Dashing = true;

        MouseClickPosition.y = transform.parent.position.y;
        transform.parent.position = MouseClickPosition;

        LeanTween.value(gameObject, transform.parent.position, MouseClickPosition, DashSpeed).setEasePunch();

        NMAgent.isStopped = true;
        NMAgent.ResetPath();
    }

    private void Update()
    {
        if (Input.GetKeyUp(ActivationKey))
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
