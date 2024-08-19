using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] bool TurnOffSpawner;
    [SerializeField] float asteroidSpawnInterval;
    [SerializeField] List<Enemy> EnemiesSpawned = new();

    public List<Collider2D> SpawnZones;
    [SerializeField] Collider2D targetZone;

    int nextAsteroidToSpawn;

    private void Start()
    {
        InvokeRepeating("SpawnAsteroid", asteroidSpawnInterval, asteroidSpawnInterval);

        List<Enemy> tempList = new();
        foreach (Enemy enemy in EnemiesSpawned)
        {
            for(int i = 0; i < enemy.NumberOfCopiesInList; i++)
            {
                tempList.Add(new Enemy(enemy));
            }
        }
        List<Enemy> shuffledList = new List<Enemy>();
        while (tempList.Any())
        {
            int rnd = Random.Range(0, tempList.Count());
            shuffledList.Add(tempList[rnd]);
            tempList.RemoveAt(rnd);
        }
        EnemiesSpawned = new(shuffledList);
        nextAsteroidToSpawn = 0;
    }

    void SpawnAsteroid()
    {
        if (TurnOffSpawner) return;
        if (PlayerMain.Instance == null)
        {
            TurnOffSpawner = true;
            return;
        }

        int spawnZoneRnd = Random.Range(0, SpawnZones.Count);
        //int enemRnd = Random.Range(0, EnemiesSpawned.Count);
        int enemRnd = nextAsteroidToSpawn;

        GameObject gob = Instantiate(EnemiesSpawned[enemRnd].EnemyPrefab, RandomPointInBounds(SpawnZones[spawnZoneRnd].bounds), Quaternion.identity);

        float scale = Random.Range(EnemiesSpawned[enemRnd].SizeRange.x, EnemiesSpawned[enemRnd].SizeRange.y);

        if (!EnemiesSpawned[enemRnd].DisableExtremeSizeDifferenceChance)
        {
            int extremeSizeRnd = Random.Range(0, 10);
            if (extremeSizeRnd == 0)
            {
                scale *= 2.5f;
            }
            else if (extremeSizeRnd == 1)
            {
                scale /= 2.5f;
            }
        }

        Rigidbody2D rb = gob.GetComponent<Rigidbody2D>();

        gob.transform.localScale = new Vector3(scale, scale, scale);
        rb.mass = scale;

        Vector2 destination = RandomPointInBounds(targetZone.bounds);
        float speed = Random.Range(EnemiesSpawned[enemRnd].SpeedRange.x, EnemiesSpawned[enemRnd].SpeedRange.y);
        rb.velocity = (destination - (Vector2)gob.transform.position).normalized * speed;

        float spinRnd = Random.Range(EnemiesSpawned[enemRnd].SpinRange.x, EnemiesSpawned[enemRnd].SpinRange.y);
        int mod = Random.Range(0, 2);
        if (mod == 1) spinRnd *= -1;
        rb.AddTorque(spinRnd);

        nextAsteroidToSpawn++;
        if (nextAsteroidToSpawn >= EnemiesSpawned.Count) nextAsteroidToSpawn = 0;
    }


    Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y));
    }

    [System.Serializable]
    public class Enemy
    {
        public int NumberOfCopiesInList;
        public GameObject EnemyPrefab;
        public Vector2 SizeRange;
        public Vector2 SpeedRange;
        public Vector2 SpinRange;
        public bool DisableExtremeSizeDifferenceChance;

        public Enemy(Enemy enemy)
        {
            EnemyPrefab = enemy.EnemyPrefab;
            SizeRange = enemy.SizeRange;
            SpeedRange = enemy.SpeedRange;
            SpinRange = enemy.SpinRange;
            DisableExtremeSizeDifferenceChance = enemy.DisableExtremeSizeDifferenceChance;
        }
    }

    void ShuffleDeckList(ref List<GameObject> deckList)
    {
        
    }
}
