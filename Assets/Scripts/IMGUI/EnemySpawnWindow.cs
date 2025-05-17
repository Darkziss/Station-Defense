using UnityEngine;

namespace StationDefense.IMGUI
{
    public class EnemySpawnWindow : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        private bool _shouldDisplayWindow = true;

        private readonly Rect _windowRect = new(15f, 15f, 420, 200);

        private const string WindowContent = "Enemy Spawn";
        private const string SpawnButtonContent = "Spawn Big Enemy";

        private const KeyCode SwitchWindowKey = KeyCode.H;

        private void Update()
        {
            if (Input.GetKeyDown(SwitchWindowKey))
                _shouldDisplayWindow = !_shouldDisplayWindow;
        }

        private void OnGUI()
        {
            if (!_shouldDisplayWindow)
                return;
            
            GUILayout.BeginVertical();

            GUILayout.Window(0, _windowRect, SpawnWindowFunction, WindowContent);
            
            GUILayout.EndHorizontal();
        }

        private void SpawnWindowFunction(int id)
        {
            if (GUILayout.Button(SpawnButtonContent, GUILayout.Width(150), GUILayout.Height(80)))
            {
                _enemySpawner.SpawnBigEnemy();
            }
        }
    }
}