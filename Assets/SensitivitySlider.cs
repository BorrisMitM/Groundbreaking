using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private void OnEnable()
    {
        GetComponent<Slider>().value = GameManager.instance.sensitivity;
    }

    public void OnValueChanged(float value)
    {
        text.text = value.ToString();
        GameManager.instance.sensitivity = value;
    }
}
