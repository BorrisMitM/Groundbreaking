using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    [SerializeField] private LayerMask lm;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, lm))
        {
            if (!shadow.activeSelf) shadow.SetActive(true);
            shadow.transform.position = hit.point;
        }
        else if(shadow.activeSelf) shadow.SetActive(false);
    }
}
