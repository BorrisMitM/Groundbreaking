using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour
{
    public void Do()
    {
        GameManager.instance.ReloadScene();
    }
}
