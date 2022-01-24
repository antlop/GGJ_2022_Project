using System.Collections.Generic;
using UnityEngine;

public class BaseComponentController : MonoBehaviour
{
    public List<Behaviour> _Components = new List<Behaviour>();

    public void Awake()
    {
        foreach (var c in _Components)
        {
            c.enabled = false;
        }
    }

    public void ActivateObject()
    {
        foreach (var c in _Components)
        {
            c.enabled = true;
        }
    }
}
