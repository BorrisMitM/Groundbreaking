using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisEmitter : MonoBehaviour
{
    [SerializeField] private GameObject debris;
    [SerializeField] public int debrisAmount = 50;
    [SerializeField] public float minVelocity = 20;
    [SerializeField] public float maxVelocity = 40;
    [SerializeField] public float radius = 10f;
    [SerializeField] public float spawnIntervall;
    [SerializeField] [Range(0,1)]public float angelaSpread = 1f;
    private float spawnIntervallTimer = 0;

    private void Start()
    {
        spawnIntervallTimer = spawnIntervall;
    }


    private void SpawnDebris()
    {
        for (int i = 0; i < debrisAmount; i++)
        {
            Vector3 offset = Random.insideUnitCircle * radius;
            Vector3 spawnPosition = transform.position + transform.right * offset.x + transform.up * offset.y;
            GameObject newDebris = Instantiate(debris, spawnPosition, Quaternion.identity);
            Vector2 insideUnitCircle = Random.insideUnitCircle * angelaSpread;
            Vector3 dir = new Vector3(insideUnitCircle.x, 1f, insideUnitCircle.y).normalized;
            newDebris.GetComponent<Rigidbody>().velocity = dir * Random.Range(minVelocity, maxVelocity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnIntervallTimer += Time.unscaledDeltaTime;
        if(spawnIntervallTimer >= spawnIntervall)
        {
            spawnIntervallTimer -= spawnIntervall;
            SpawnDebris();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);
        Gizmos.DrawSphere(transform.position, 1f);
    }
}
