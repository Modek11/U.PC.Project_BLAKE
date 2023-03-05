using UnityEngine;
using TMPro;

[System.Serializable]
public class Wave
{
    [SerializeField] private string waveName;
    public int enemiesNumber;
}
public class WaveManager : MonoBehaviour
{
    [Header("WavePreset")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Wave[] waves;

    [Header("WaveUI")]
    [SerializeField] private TextMeshProUGUI waveCountdown;
    [SerializeField] private GameObject youWonScreen;
    [SerializeField] private GameObject gameOverScreen;

    [Header("WaveIteration")]
    private Wave _currentWave;
    private int _currentWaveIndex;
    private WaveState _currentWaveState;
    private int _enemiesAlive;

    [Header("WaveCountdownVariables")]
    private float _waveCountdownValue;
    private float _timeBetweenWaves;
    
    private GameObject _playerRef;

    private void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _timeBetweenWaves = 5f;
        _waveCountdownValue = _timeBetweenWaves;
        
        _currentWave = waves[_currentWaveIndex];
        _currentWaveState = WaveState.PreWave;
        Time.timeScale = 1;
    }
    
    private void Update()
    {
        switch (_currentWaveState)
        {
            case WaveState.PreWave:
                {
                    waveCountdown.gameObject.SetActive(true);
                    WaveCountdown();
                    break;
                }

            case WaveState.WaveStart:
                {
                    SpawnEnemies();
                    break;
                }

            case WaveState.WaveInProgress:
                {
                    CheckForEnemies();
                    CheckForPlayer();
                    break;
                }

            case WaveState.WaveEnd:
                {
                    CheckForNextWave();
                    break;
                }
        }
    }

    private void SpawnEnemies()
    {
        int enemiesToSpawn = _currentWave.enemiesNumber;

        while (enemiesToSpawn > 0)
        {
            Vector3 positionOffset = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));

            Transform randomEnemyPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
            GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            Instantiate(randomEnemyPrefab, randomEnemyPoint.position + positionOffset, Quaternion.identity);

            enemiesToSpawn--;
            
        }
        _currentWaveState = WaveState.WaveInProgress;
        _enemiesAlive = _currentWave.enemiesNumber;
    }

    public void DecreaseEnemyCount()
    {
        _enemiesAlive--;
    }

    private void WaveCountdown()
    {
        waveCountdown.text = _waveCountdownValue.ToString("0");
        _waveCountdownValue -= 1 * Time.deltaTime;

        if (_waveCountdownValue <= 0)
        {
            _waveCountdownValue = 0;
            _currentWaveState = WaveState.WaveStart;

            _waveCountdownValue = _timeBetweenWaves;
            waveCountdown.gameObject.SetActive(false);
        }
    }

    private void CheckForEnemies()
    {
        if (IsEnemyAlive()) return;

        _currentWaveState = WaveState.WaveEnd;
    }

    private bool IsEnemyAlive()
    {
        return _enemiesAlive != 0;
    }

    private void CheckForPlayer()
    {
        if (IsPlayerAlive()) return;
        
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }

    private bool IsPlayerAlive()
    {
        return _playerRef != null;
    }

    private void CheckForNextWave()
    {
        if (_currentWaveIndex + 1 <= waves.Length - 1)
        {
            _currentWaveIndex++;
            _currentWave = waves[_currentWaveIndex];
            _currentWaveState = WaveState.PreWave;
        }
        else
        {
            youWonScreen.gameObject.SetActive(true);
        }
    }
}
