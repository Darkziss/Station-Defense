using System.Collections;
using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private BaseEnemy _enemyPrefab;
        [SerializeField] private BaseEnemy _fastEnemyPrefab;
        [SerializeField] private BaseEnemy _strongEnemyPrefab;

        [SerializeField] private bool _shouldSpawn = false;

        private Coroutine _spawnCoroutine;

        private readonly WaitForSeconds _spawnDelay = new(1.5f);

        private const float minX = -maxX;
        private const float maxX = 10f;

        private const float minY = -maxY;
        private const float maxY = 6f;

        public void StartSpawn()
        {
            if (_shouldSpawn)
                return;

            _shouldSpawn = true;

            _spawnCoroutine = StartCoroutine(SpawnEnemiesRandomly());
        }

        public void StopSpawn()
        {
            if (!_shouldSpawn)
                return;

            _shouldSpawn = false;

            StopCoroutine(_spawnCoroutine);
        }

        private IEnumerator SpawnEnemiesRandomly()
        {
            while (true)
            {
                yield return _spawnDelay;

                BaseEnemy enemyPrefab = _enemyPrefab; //GetRandomEnemy();
                Vector3 position = GetRandomPosition();
                ColorTeam team = GetRandomTeam();

                BaseEnemy enemy = PoolStorage.GetFromPool(enemyPrefab.EnemyName, enemyPrefab, position,
                    Quaternion.identity);
                
                enemy.Init(team);
            }
        }

        private static bool GetRandomBool() => Random.Range(0, 2) != 0;

        private BaseEnemy GetRandomEnemy()
        {
            int enemyType = Random.Range(1, 4);

            return enemyType switch
            {
                1 => _enemyPrefab,
                2 => _fastEnemyPrefab,
                3 => _strongEnemyPrefab,
                _ => _enemyPrefab
            };
        }

        private static Vector3 GetRandomPosition()
        {
            bool vertical = GetRandomBool();
            Vector3 position = new();

            if (vertical)
            {
                position.x = Random.Range(minX, maxX);
                position.y = GetRandomBool() ? minY : maxY;
            }
            else
            {
                position.x = GetRandomBool() ? minX : maxX;
                position.y = Random.Range(minY, maxY);
            }

            return position;
        }

        private static ColorTeam GetRandomTeam() => (ColorTeam)Random.Range(0, 4);
    }
}