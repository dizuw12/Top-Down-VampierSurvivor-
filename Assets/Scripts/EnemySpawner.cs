using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private Transform target;
    [SerializeField] private float spawnTime = 3f;
    [SerializeField] private float spawnRate;
    [SerializeField] private float distance = 10f;
   
    
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        spawnRate = spawnTime;
    }

    
    void Update()
    {
        spawnRate -= Time.deltaTime;

        if (spawnRate <= 0)
        {
            SpawnEnemy();
            spawnRate = spawnTime;
        }
       
        

    }
    private void SpawnEnemy()
    {
        Vector2 randomRange = Random.insideUnitCircle.normalized * distance;
        Vector2 randomPosition = (Vector2)target.position + randomRange;
        Instantiate(enemyToSpawn, randomPosition, Quaternion.identity);


    }
}
