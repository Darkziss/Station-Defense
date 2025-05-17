using UnityEngine;

namespace StationDefense.IMGUI
{
    public class EnemySpawnWindow : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        private readonly Rect _windowRect = new(15f, 15f, 420, 200);
        private readonly Rect _spawnButtonRect = new(30f, 30f, 150, 80);

        private const string WindowContent = "Enemy Spawn";
        private const string SpawnButtonContent = "Spawn Big Enemy";
        
        private void OnGUI()
        {
            GUI.Window(0, _windowRect, SpawnWindowFunction, WindowContent);
        }

        private void SpawnWindowFunction(int id)
        {
            if (GUI.Button(_spawnButtonRect, SpawnButtonContent))
            {
                _enemySpawner.SpawnBigEnemy();
            }
        }
    }
}