using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class s_UpdateUIBars : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void UpdateBar(float currentHpOrComfort, float maXHpOrComfort)
    {
        slider.DOValue(currentHpOrComfort / maXHpOrComfort, 1f);
    }
}
