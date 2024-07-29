using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] _spawnPoints;    

    private float _repeatRate = 2f;
    private int _poolCapacity = 30;
    private int _poolMaxSize = 30;
    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(            
            createFunc: () => CreateEnemy(),
            actionOnGet: (enemy) => EnableEnemy(enemy),
            actionOnRelease: (enemy) => DisableEnemy(enemy),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(RepeatGetEnemy(_repeatRate));
    }

    private IEnumerator RepeatGetEnemy(float repeatRate)
    {
        var wait = new WaitForSeconds(repeatRate);

        while (enabled)
        {
            GetEnemy();
            yield return wait;
        }
    }

    private Enemy CreateEnemy()
    {
        SpawnPoint spawnPoint = GetSpawnPoint();

        Enemy spawnPointEnemy = spawnPoint.Enemy;

        Enemy enemy = Instantiate(spawnPointEnemy, spawnPoint.transform.position, Quaternion.identity);
        enemy.Initialize(spawnPoint.Target, spawnPoint.transform.position);

        return enemy;
    }

    private SpawnPoint GetSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
    }

    private void EnableEnemy(Enemy enemy)
    {
        enemy.transform.position = enemy.StartPosition;        

        enemy.gameObject.SetActive(true);

        enemy.Died += ReleaseEnemy;
    }    

    private void DisableEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        enemy.Died -= ReleaseEnemy;
    }

    private void GetEnemy()
    {
        _pool.Get();
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
    }
}
