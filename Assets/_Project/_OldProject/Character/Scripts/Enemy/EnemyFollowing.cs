using UnityEngine;

public class EnemyFollowing : MonoBehaviour
{
    private Transform _playerTransform;
    private float _enemySpeed = 12f;
    private Rigidbody _rigidbody;
    private Transform _thisTransform;

    private WaveManager _waveManagerScript;

    void Awake()
    {
        _playerTransform = GameObject.Find("Player").transform;
        _rigidbody = GetComponent<Rigidbody>();

        _waveManagerScript = GameObject.Find("WaveManager").GetComponent<WaveManager>();
    }

    void FixedUpdate()
    {
        if (_playerTransform == null) return;
        
        _thisTransform = transform;
        _thisTransform.LookAt(_playerTransform);
        _rigidbody.MovePosition(Vector3.MoveTowards(_thisTransform.position, _playerTransform.position, _enemySpeed * Time.fixedDeltaTime));
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sensor"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        _waveManagerScript.DecreaseEnemyCount();
    }
}
