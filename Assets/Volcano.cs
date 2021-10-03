using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    public static Volcano instance;
    [SerializeField] private float timeToGetTheCollectible;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        timer = timeToGetTheCollectible;
    }
    private void Update()
    {
        if (GameManager.instance.explorationMode) return;
        timer -= Time.deltaTime;
        if (timer < 0) GameManager.instance.Lose();
    }

    public void ItemCollected()
    {
        timer = timeToGetTheCollectible;
    }
}
