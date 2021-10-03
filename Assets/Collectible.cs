using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Volcano.instance.ItemCollected();
            GameManager.instance.CountUp();
            FindObjectOfType<LevelGenerator>().SpawnNewCollectible();
            Destroy(gameObject);
        }
    }
}
