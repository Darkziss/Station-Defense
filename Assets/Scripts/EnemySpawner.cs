using System.Collections;
using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab;

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
            static bool RandomBool() => Random.Range(0, 2) != 0;
            
            while (true)
            {
                yield return _spawnDelay;

                bool vertical = RandomBool();
                Vector3 position = new();

                if (vertical)
                {
                    position.x = Random.Range(minX, maxX);
                    position.y = RandomBool() ? minY : maxY;
                }
                else
                {
                    position.x = RandomBool() ? minX : maxX;
                    position.y = Random.Range(minY, maxY);
                }

                Enemy enemy = PoolStorage.GetFromPool(nameof(Enemy), _enemyPrefab, position,
                    Quaternion.identity);
                enemy.Activate();
            }
        }
    }
}