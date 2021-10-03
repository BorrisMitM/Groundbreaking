using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Object Refernces")]
    [SerializeField] private float cellSize = 10f;
    [SerializeField] private int gridSize = 30;
    [SerializeField] private float wallHeight = 40f;
    [SerializeField] private GameObject lava;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject safeSpace;
    [SerializeField] private GameObject debrisEmitter;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject collectibles;
    [Header("GenerationStuff")]
    [SerializeField] private float noiseScale = 0.268f;
    [SerializeField] private float safeSpaceBorder = 0.8f;
    [SerializeField] private float debrisEmitterBorder = 0.3f;
    [SerializeField] private float fireballEmitterBorder = 0.3f;
    private float levelSize;
    private float noiseOffset;
    bool characterSpawned = false;
    private GameObject characterRef;
    private int lastSpawnLocation = 0;
    // Start is called before the first frame update
    void Start()
    {
        noiseOffset = Random.Range(0, 1000f);
        levelSize = cellSize * gridSize;
        SetFrame();
        PlaceObjects();
        SpawnNewCollectible();
    }

    public void SpawnNewCollectible()
    {
        int newSpawnLocation = Random.Range(0, 4);
        if(newSpawnLocation == lastSpawnLocation)
        {
            newSpawnLocation += 2;
            newSpawnLocation %= 4;
        }
        lastSpawnLocation = newSpawnLocation;
        if(newSpawnLocation == 0)
        {
            GameObject newCollectible = Instantiate(collectibles, new Vector3(-levelSize/2f + cellSize / 2f,  13, -levelSize / 2f + cellSize / 2f), Quaternion.identity, transform);
        }
        else if(newSpawnLocation == 1)
        {
            GameObject newCollectible = Instantiate(collectibles, new Vector3(levelSize / 2f - cellSize / 2f, 13, -levelSize / 2f + cellSize / 2f), Quaternion.identity, transform);
        }
        else if(newSpawnLocation == 2)
        {
            GameObject newCollectible = Instantiate(collectibles, new Vector3(levelSize / 2f - cellSize / 2f, 13, levelSize / 2f - cellSize / 2f), Quaternion.identity, transform);
        }
        else if(newSpawnLocation == 3)
        {
            GameObject newCollectible = Instantiate(collectibles, new Vector3(-levelSize / 2f + cellSize / 2f, 13, levelSize / 2f - cellSize / 2f), Quaternion.identity, transform);
        }
    }
    private void PlaceObjects()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector3 position = new Vector3(-levelSize / 2 + i * cellSize + cellSize / 2f, 0, -levelSize / 2 + j * cellSize + cellSize / 2f);
                if ((i == 0 && j == 0) || (i == 0 && j == gridSize - 1) || (i == gridSize - 1 && j == 0) || (i == gridSize - 1 && j == gridSize - 1))
                {
                    GameObject newSafeSpace = Instantiate(safeSpace, position + Vector3.up * 5, Quaternion.identity, transform);
                    continue;                
                }
                float value = Mathf.PerlinNoise(i * noiseScale + noiseOffset, j * noiseScale + noiseOffset);
                //place safe spaces
                if (value > safeSpaceBorder)
                {
                    if (!characterSpawned)
                    {
                        characterRef = Instantiate(character, position + Vector3.up * 10, Quaternion.identity);
                        characterSpawned = true;
                    }

                    GameObject newSafeSpace = Instantiate(safeSpace, position + Vector3.up * 5, Quaternion.identity, transform);
                }
                //place debris emitter
                else if(value > debrisEmitterBorder)
                {
                    GameObject newDebrisEmitter = Instantiate(debrisEmitter, position, Quaternion.Euler(-90,0,0), transform);
                    DebrisEmitter de = newDebrisEmitter.GetComponent<DebrisEmitter>();
                    de.debrisAmount = Random.Range(1, 3);
                    de.minVelocity = Random.Range(2, 7);
                    de.maxVelocity = de.minVelocity + Random.Range(2, 6);
                    de.radius = cellSize / 2f;
                    de.spawnIntervall = Random.Range(5, 9);
                }
                else if(value > fireballEmitterBorder)
                {
                    GameObject newFireball = Instantiate(fireball, position + Vector3.down * 2f, Quaternion.identity, transform);
                    Fireball fb = newFireball.GetComponent<Fireball>();
                    fb.height = Random.Range(5, 20);
                    fb.width = Random.Range(0, cellSize);
                    fb.duration = fb.height / 5;
                    fb.breakTime = Random.Range(0, 3);
                }
            }
        }
    }

    private void SetFrame()
    {
        lava.transform.position = Vector3.zero;
        lava.transform.localScale = new Vector3(levelSize / 10f, 1, levelSize / 10f);

        leftWall.transform.position = new Vector3(-levelSize / 2f, wallHeight / 2f, 0);
        leftWall.transform.localScale = new Vector3(wallHeight, 1, levelSize) / 10f;

        rightWall.transform.position = new Vector3(levelSize / 2f, wallHeight / 2f, 0);
        rightWall.transform.localScale = new Vector3(wallHeight, 1, levelSize) / 10f;

        frontWall.transform.position = new Vector3(0, wallHeight / 2f, levelSize / 2f);
        frontWall.transform.localScale = new Vector3(levelSize, 1, wallHeight) / 10f;

        backWall.transform.position = new Vector3(0, wallHeight / 2f, -levelSize / 2f);
        backWall.transform.localScale = new Vector3(levelSize, 1, wallHeight) / 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
