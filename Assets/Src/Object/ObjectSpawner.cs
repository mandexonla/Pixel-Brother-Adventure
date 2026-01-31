using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{

    public enum ObjectType { SmallFruit, BigFruit, Enemy }

    public Tilemap tilemap;
    public GameObject[] objectsPrefabs; //0=smallFruit, 1 = BigFruit, 2= Enemy
    public float bigFruitProbibility = 0.2f;// 20% chance of spawing big fruit
    public float enemyProbibility = 1f;
    public int maxObjects = 5;
    public float fruitLifeTime = 10f;//only for fruit
    public float spawnInterval = 0.5f;

    private List<Vector3> validSpawnPositions = new List<Vector3>();
    private List<GameObject> spawnObjects = new List<GameObject>();
    private bool isSpawning = false;


    private List<Vector3> groundPositions = new();
    private List<Vector3> wallLeftPositions = new();
    private List<Vector3> wallRightPositions = new();


    // Start is called before the first frame update
    void Start()
    {
        //Gather valid Positions
        GatherValidPositions();
        StartCoroutine(SpawnObjectsIfNeeded());
    }

    // Update is called once per frame
    void Update()
    {
        if (!tilemap.gameObject.activeInHierarchy)
        {
            LevelChange();
        }
        if (!isSpawning && ActiveObjectsCount() < maxObjects)
        {
            StartCoroutine(SpawnObjectsIfNeeded());
        }
    }

    private void LevelChange()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        GatherValidPositions();
        DestroyAllSpawnObjects();
    }

    private int ActiveObjectsCount()
    {
        spawnObjects.RemoveAll(item => item == null);
        return spawnObjects.Count;
    }

    private IEnumerator SpawnObjectsIfNeeded()
    {
        isSpawning = true;
        while (ActiveObjectsCount() < maxObjects)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false;
    }

    private bool PositionHasObject(Vector3 positionToCheck)
    {
        return spawnObjects.Any(checkObj => checkObj && Vector3.Distance(checkObj.transform.position,
            positionToCheck) < 1.0f);
    }

    private ObjectType RandomObjectType()
    {
        float randomChoice = Random.value;

        if (randomChoice <= enemyProbibility)
        {
            return ObjectType.Enemy;
        }
        else if (randomChoice <= (enemyProbibility + bigFruitProbibility))
        {
            return ObjectType.BigFruit;
        }
        else
        {
            return ObjectType.SmallFruit;
        }
    }

    private void SpawnObject()
    {
        if (validSpawnPositions.Count == 0) return;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound && validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 pontentialPosition = validSpawnPositions[randomIndex];
            Vector3 leftPosition = pontentialPosition + Vector3.left;
            Vector3 rightPosition = pontentialPosition + Vector3.right;

            if (!PositionHasObject(leftPosition) && !PositionHasObject((rightPosition)))
            {
                spawnPosition = pontentialPosition;
                validPositionFound = true;
            }

            validSpawnPositions.RemoveAt(randomIndex);
        }

        if (validPositionFound)
        {
            ObjectType objectType = RandomObjectType();
            GameObject gameObject = Instantiate(objectsPrefabs[(int)objectType], spawnPosition, Quaternion.identity);
            spawnObjects.Add(gameObject);

            //Destroy fruit only after time
            if (objectType != ObjectType.Enemy)
            {
                StartCoroutine(DestroyObjectAfterTime(gameObject, fruitLifeTime));
            }
        }
    }

    private IEnumerator DestroyObjectAfterTime(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);

        if (gameObject)
        {
            spawnObjects.Remove(gameObject);
            validSpawnPositions.Add(gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    private void DestroyAllSpawnObjects()
    {
        foreach (GameObject obj in spawnObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnObjects.Clear();
    }

    private void GatherValidPositions()
    {
        validSpawnPositions.Clear();

        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);

                if (!tilemap.HasTile(cell)) continue;

                Vector3Int above = new Vector3Int(x, y + 1, 0);
                if (tilemap.HasTile(above)) continue; // There are tiles above, not the ground.

                Vector3 worldPos = tilemap.GetCellCenterWorld(above);

                validSpawnPositions.Add(worldPos);
            }
        }
    }

}
