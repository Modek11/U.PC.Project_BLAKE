using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float timeForNextEnemyToSpawn;    
    private float elapsedTimeForNextEnemyToSpawn;
    private Vector3 _spawnPosition;
    private float distance;

    void Start()
    {
        elapsedTimeForNextEnemyToSpawn = timeForNextEnemyToSpawn;
    }

    void Update()
    {
        SpawnEnemy();
        elapsedTimeForNextEnemyToSpawn -= Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        if(elapsedTimeForNextEnemyToSpawn > 0) return;

        do
        {
            _spawnPosition = new Vector3(Random.Range(-24f, 24f), 0f, Random.Range(-24f, 24f));
            distance = Vector3.Distance(Vector3.zero, _spawnPosition);
        } while (distance < 20f);
        
        Instantiate(enemyPrefab[0],_spawnPosition, Quaternion.identity);

        elapsedTimeForNextEnemyToSpawn = timeForNextEnemyToSpawn;
    }

}
