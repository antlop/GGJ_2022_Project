using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawSpin : MonoBehaviour
{
    public float spinRate = 250;

    private void LateUpdate()
    {
        transform.Rotate(Vector3.right, spinRate * Time.deltaTime);
    }
}
