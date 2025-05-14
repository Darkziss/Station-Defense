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
        private Coroutine _spawnGroupCoroutine;

        private readonly WaitForSeconds _singleSpawnDelay = new(1.5f);
        private readonly WaitForSeconds _groupSpawnDelay = new(4f);

        private readonly WaitForSeconds _enemySpawnDelay = new(1.3f);

        private const float firstSpawnDelay = 1f;

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
            StopCoroutine(_spawnGroupCoroutine);

            _spawnCoroutine = null;
            _spawnGroupCoroutine = null;
        }

        private IEnumerator SpawnEnemiesRandomly()
        {
            yield return new WaitForSeconds(firstSpawnDelay);
            
            while (true)
            {
                bool isGroup = RandomUtility.GetRandomBool();

                if (isGroup)
                    SpawnRandomGroup();
                else
                    SpawnRandomEnemy();

                yield return isGroup ? _groupSpawnDelay : _singleSpawnDelay;
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
            const int min = 0;
            int index = Random.Range(min, _groups.Length);

            _spawnGroupCoroutine = StartCoroutine(SpawnGroup(_groups[index]));
        }

        private IEnumerator SpawnGroup(EnemyGroup group)
        {
            for (int i = 0; i < group.BaseEnemyCount; i++)
            {
                SpawnEnemyByPrefab(_enemyPrefab);

                yield return _enemySpawnDelay;
            }

            for (int i = 0; i < group.FastEnemyCount; i++)
            {
                SpawnEnemyByPrefab(_fastEnemyPrefab);

                yield return _enemySpawnDelay;
            }

            for (int i = 0; i < group.BigEnemyCount; i++)
            {
                SpawnEnemyByPrefab(_bigEnemyPrefab);

                yield return _enemySpawnDelay;
            }
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