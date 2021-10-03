using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSize : MonoBehaviour
{
    [SerializeField] float minSize = 1f;
    [SerializeField] float maxSize = 3f;
    private void Awake()
    {
        transform.localScale = new Vector3(Random.Range(minSize, maxSize), 1, Random.Range(minSize, maxSize));
    }
}
