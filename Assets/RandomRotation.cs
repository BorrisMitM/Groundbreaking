using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.rotation = Quaternion.Euler(Random.Range(0, 4) * 90f, Random.Range(0, 4) * 90f, Random.Range(0, 4) * 90f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
