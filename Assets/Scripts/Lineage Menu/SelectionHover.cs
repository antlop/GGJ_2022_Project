using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionHover : MonoBehaviour
{
    public void ChangePositionTo(RectTransform tran)
    {
        this.GetComponent<Image>().rectTransform.anchoredPosition = tran.anchoredPosition;

        Debug.Log("CLICKED ME HKERER");
    }
}
