using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    [SerializeField] public float height;
    [SerializeField] public float width;
    [SerializeField] public float duration;
    [SerializeField] public float breakTime;
    [SerializeField] private GameObject ball;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    // Update is called once per frame
    IEnumerator Move()
    {
        while (true)
        {
            float progress = timer / duration;
            if (progress >= 1)
            {
                timer = 0;
                yield return new WaitForSeconds(breakTime);
            }
            float t = Mathf.Sin(progress * Mathf.PI);
            float currentHeight = Mathf.Lerp(0, height, t);
            float currentWidth = Mathf.Lerp(-width, width, progress);
            ball.transform.localPosition = new Vector3(currentWidth, currentHeight, 0);// Vector3.Lerp(Vector3.zero, Vector3.up * height, t);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
