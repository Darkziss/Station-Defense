using System.Collections;
using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Enemy _fastEnemyPrefab;
        [SerializeField] private Enemy _strongEnemyPrefab;

        [SerializeField] private bool _shouldSpawn = false;

        private Coroutine _spawnCoroutine;

        private readonly WaitForSeconds _spawnDelay = new(1.5f);

        private const float minX = -maxX;
        private const float maxX = 10f;

        private const float minY = -maxY;
        private const float maxY = 6f;

        private const string baseEnemyName = "BaseEnemy";
        private const string fastEnemyName = "FastEnemy";
        private const string strongEnemyName = "StrongEnemy";

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

                (string, Enemy) enemyData = GetRandomEnemy();
                Vector3 position = GetRandomPosition();
                ColorTeam team = GetRandomTeam();

                Enemy enemy = PoolStorage.GetFromPool(enemyData.Item1, enemyData.Item2, position,
                    Quaternion.identity);
                
                enemy.Init(team);
            }
        }

        private static bool GetRandomBool() => Random.Range(0, 2) != 0;

        private (string, Enemy) GetRandomEnemy()
        {
            int enemyType = Random.Range(1, 4);

            return enemyType switch
            {
                1 => (baseEnemyName, _enemyPrefab),
                2 => (fastEnemyName, _fastEnemyPrefab),
                3 => (strongEnemyName, _strongEnemyPrefab),
                _ => (baseEnemyName, _enemyPrefab)
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