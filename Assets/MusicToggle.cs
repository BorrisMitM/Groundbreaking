using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Toggle>().isOn = !MusicManager.instance.GetComponent<AudioSource>().mute;
    }
    public void OnValueChanged(bool value)
    {
        MusicManager.instance.GetComponent<AudioSource>().mute = !value;
    }
}
