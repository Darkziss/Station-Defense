using System.Collections;
using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private BaseEnemy _enemyPrefab;
        [SerializeField] private FastEnemy _fastEnemyPrefab;
        [SerializeField] private BigEnemy _bigEnemyPrefab;

        [SerializeField] private EnemyGroup[] _groups;

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

                bool isGroup = RandomUtility.GetRandomBool();

                if (isGroup)
                    SpawnRandomGroup();
                else
                    SpawnRandomEnemy();
            }
        }

        private void SpawnRandomEnemy()
        {
            Enemy prefab = GetRandomEnemy();

            SpawnEnemyByPrefab(prefab);
        }

        private void SpawnEnemyByPrefab(Enemy prefab)
        {
            Vector3 position = GetRandomPosition();
            ColorTeam team = GetRandomTeam();

            Enemy enemy = PoolStorage.GetFromPool(prefab.EnemyName, prefab, position,
                Quaternion.identity);

            enemy.Init(team);
        }

        private void SpawnRandomGroup()
        {
            void SpawnMultipleEnemy(Enemy prefab, int spawnCount)
            {
                for (int i = 0; i < spawnCount; i++)
                    SpawnEnemyByPrefab(prefab);
            }
            
            const int min = 0;

            int index = Random.Range(min, _groups.Length);

            EnemyGroup group = _groups[index];

            if (group.BaseEnemyCount > 0)
                SpawnMultipleEnemy(_enemyPrefab, group.BaseEnemyCount);

            if (group.FastEnemyCount > 0)
                SpawnMultipleEnemy(_fastEnemyPrefab, group.FastEnemyCount);

            if (group.BigEnemyCount > 0)
                SpawnMultipleEnemy(_bigEnemyPrefab, group.BigEnemyCount);
        }

        private Enemy GetRandomEnemy()
        {
            int enemyType = Random.Range(1, 4);

            return enemyType switch
            {
                1 => _enemyPrefab,
                2 => _fastEnemyPrefab,
                3 => _bigEnemyPrefab,
                _ => _enemyPrefab
            };
        }

        private static Vector3 GetRandomPosition()
        {
            bool vertical = RandomUtility.GetRandomBool();
            Vector3 position = new();

            if (vertical)
            {
                position.x = Random.Range(minX, maxX);
                position.y = RandomUtility.GetRandomBool() ? minY : maxY;
            }
            else
            {
                position.x = RandomUtility.GetRandomBool() ? minX : maxX;
                position.y = Random.Range(minY, maxY);
            }

            return position;
        }

        private static ColorTeam GetRandomTeam() => (ColorTeam)Random.Range(0, 4);
    }
}