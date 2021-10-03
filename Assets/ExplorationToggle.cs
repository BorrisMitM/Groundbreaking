using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorationToggle : MonoBehaviour
{
    public GameObject timer;
    private void OnEnable()
    {
        GetComponent<Toggle>().isOn = GameManager.instance.explorationMode;
    }
    public void OnValueChanged(bool value)
    {
        GameManager.instance.explorationMode = value;
        timer.SetActive(!value);
    }
}
